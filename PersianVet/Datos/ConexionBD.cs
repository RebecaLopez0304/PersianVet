using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PersianVet.Datos;

/// <summary>
/// Crea las conexiones usadas para trabajar con SQL Server.
/// </summary>
public class ConexionBD
{
    private const string CadenaPredeterminada =
        "Server=(localdb)\\MSSQLLocalDB;Database=PersianVetDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;Connect Timeout=5;";

    public string CadenaConexion { get; }

    public ConexionBD()
    {
        CadenaConexion = LeerCadenaConexion();
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
