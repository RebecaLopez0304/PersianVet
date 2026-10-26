using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PersianVet.Datos;

/// <summary>
/// Crea las conexiones usadas para trabajar con SQL Server.
/// Si el servidor configurado no responde prueba otros comunes y, si la base no existe, la crea con el script.
/// </summary>
public class ConexionBD
{
    private const string CadenaPredeterminada =
        "Server=localhost;Database=PersianVetDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;Connect Timeout=15;";

    private const string NombreBase = "PersianVetDB";

    // Servidores que se prueban, en orden, cuando el configurado no sirve.
    private static readonly string[] ServidoresAlternos = ["localhost", "(localdb)\\MSSQLLocalDB", ".\\SQLEXPRESS"];

    public string CadenaConexion { get; }

    public ConexionBD()
    {
        CadenaConexion = ResolverCadena();
    }

    /// <summary>
    /// Devuelve una conexión lista para abrirse.
    /// </summary>
    public SqlConnection CrearConexion()
    {
        return new SqlConnection(CadenaConexion);
    }

    /// <summary>
    /// Crea un contexto de Entity Framework Core de vida corta (uno por operación).
    /// </summary>
    public PersianVetContext CrearContexto()
    {
        DbContextOptions<PersianVetContext> opciones = new DbContextOptionsBuilder<PersianVetContext>()
            .UseSqlServer(CadenaConexion)
            .Options;
        return new PersianVetContext(opciones);
    }

    private static string ResolverCadena()
    {
        string configurada = LeerCadenaConexion();
        List<string> candidatas = [configurada];
        foreach (string servidor in ServidoresAlternos)
        {
            SqlConnectionStringBuilder alterna = new(configurada) { DataSource = servidor };
            if (!candidatas.Contains(alterna.ConnectionString))
            {
                candidatas.Add(alterna.ConnectionString);
            }
        }

        // 1. Algún servidor que ya tenga la base.
        foreach (string candidata in candidatas)
        {
            if (PuedeConectar(candidata, NombreBase))
            {
                return candidata;
            }
        }

        // 2. Un servidor alcanzable sin la base: se crea con el script.
        foreach (string candidata in candidatas)
        {
            if (PuedeConectar(candidata, "master") && CrearBaseDatos(candidata))
            {
                return candidata;
            }
        }

        return configurada;
    }

    private static bool PuedeConectar(string cadena, string baseDatos)
    {
        try
        {
            SqlConnectionStringBuilder prueba = new(cadena) { InitialCatalog = baseDatos, ConnectTimeout = 3 };
            using SqlConnection conexion = new(prueba.ConnectionString);
            conexion.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Ejecuta BaseDatos\PersianVetDB.sql (copiado junto al ejecutable) para crear la base y sus datos.
    /// </summary>
    private static bool CrearBaseDatos(string cadena)
    {
        try
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "BaseDatos", "PersianVetDB.sql");
            if (!File.Exists(ruta))
            {
                return false;
            }

            SqlConnectionStringBuilder master = new(cadena) { InitialCatalog = "master", ConnectTimeout = 15 };
            using SqlConnection conexion = new(master.ConnectionString);
            conexion.Open();

            // El script separa sus bloques con líneas "GO", que SqlClient no entiende.
            string[] bloques = Regex.Split(File.ReadAllText(ruta), @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (string bloque in bloques.Where(b => !string.IsNullOrWhiteSpace(b)))
            {
                using SqlCommand comando = new(bloque, conexion) { CommandTimeout = 60 };
                comando.ExecuteNonQuery();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string LeerCadenaConexion()
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, "configuracion.json");
        if (!File.Exists(ruta))
        {
            return CadenaPredeterminada;
        }

        try
        {
            string contenido = File.ReadAllText(ruta);
            using JsonDocument documento = JsonDocument.Parse(contenido);
            string? cadena = documento.RootElement.GetProperty("CadenaConexion").GetString();
            return string.IsNullOrWhiteSpace(cadena) ? CadenaPredeterminada : cadena;
        }
        catch
        {
            return CadenaPredeterminada;
        }
    }
}
