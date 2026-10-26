using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PersianVet.Datos;

/// <summary>
/// Crea las conexiones y los contextos de Entity Framework Core usados para trabajar con SQL Server.
/// </summary>
/// <remarks>
/// Si el servidor de configuracion.json no responde, prueba otros servidores comunes y, si ninguno tiene
/// la base de datos, la crea con los scripts de la carpeta BaseDatos.
/// </remarks>
public class ConexionBD
{
    private const string CadenaPredeterminada =
        "Server=localhost;Database=PersianVetDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;Connect Timeout=15;";

    private const string NombreBase = "PersianVetDB";

    // Se prueban en este orden cuando el servidor de configuracion.json no responde.
    private static readonly string[] ServidoresAlternos = ["localhost", "(localdb)\\MSSQLLocalDB", ".\\SQLEXPRESS"];

    /// <summary>
    /// Obtiene la cadena de conexión del servidor que respondió y tiene la base de datos.
    /// </summary>
    public string CadenaConexion { get; }

    /// <summary>
    /// Busca un servidor disponible; puede tardar unos segundos si el configurado no responde.
    /// </summary>
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
    /// Crea la base de datos ejecutando los scripts de la carpeta BaseDatos copiados junto al ejecutable:
    /// primero la estructura (PersianVetDB.sql) y después los datos de prueba (DatosPrueba.sql), si existe.
    /// </summary>
    /// <param name="cadena">Cadena de conexión de un servidor alcanzable.</param>
    /// <returns><see langword="true"/> si la base se creó correctamente.</returns>
    private static bool CrearBaseDatos(string cadena)
    {
        try
        {
            string carpeta = Path.Combine(AppContext.BaseDirectory, "BaseDatos");
            string estructura = Path.Combine(carpeta, "PersianVetDB.sql");
            if (!File.Exists(estructura))
            {
                return false;
            }

            SqlConnectionStringBuilder master = new(cadena) { InitialCatalog = "master", ConnectTimeout = 15 };
            using SqlConnection conexion = new(master.ConnectionString);
            conexion.Open();

            EjecutarScript(conexion, estructura);
            string datosPrueba = Path.Combine(carpeta, "DatosPrueba.sql");
            if (File.Exists(datosPrueba))
            {
                EjecutarScript(conexion, datosPrueba);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void EjecutarScript(SqlConnection conexion, string ruta)
    {
        // Los scripts separan sus bloques con líneas "GO", que SqlClient no entiende.
        string[] bloques = Regex.Split(File.ReadAllText(ruta), @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        foreach (string bloque in bloques.Where(b => !string.IsNullOrWhiteSpace(b)))
        {
            using SqlCommand comando = new(bloque, conexion) { CommandTimeout = 60 };
            comando.ExecuteNonQuery();
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
