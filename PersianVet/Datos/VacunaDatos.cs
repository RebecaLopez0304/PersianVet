using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de vacunas en SQL Server.
/// </summary>
public class VacunaDatos : IVacunaDatos
{
    private readonly ConexionBD conexionBD;

    public VacunaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Vacuna> Listar(int? idMascota = null, bool soloProximas = false)
    {
        const string sql = """
            SELECT v.IdVacuna, v.IdMascota, m.Nombre, v.NombreVacuna,
                   v.FechaAplicacion, v.ProximaDosis, v.Lote
            FROM Vacunas v
            INNER JOIN Mascotas m ON m.IdMascota = v.IdMascota
            WHERE (@IdMascota IS NULL OR v.IdMascota = @IdMascota)
              AND (@SoloProximas = 0 OR v.ProximaDosis BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)))
            ORDER BY v.FechaAplicacion DESC;
            """;

        List<Vacuna> vacunas = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdMascota", idMascota.HasValue ? idMascota.Value : DBNull.Value);
        comando.Parameters.AddWithValue("@SoloProximas", soloProximas);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            vacunas.Add(new Vacuna
            {
                IdVacuna = lector.GetInt32(0),
                IdMascota = lector.GetInt32(1),
                Mascota = lector.GetString(2),
                NombreVacuna = lector.GetString(3),
                FechaAplicacion = lector.GetDateTime(4),
                ProximaDosis = lector.IsDBNull(5) ? null : lector.GetDateTime(5),
                Lote = lector.GetString(6)
            });
        }

        return vacunas;
    }

    public int Guardar(Vacuna vacuna)
    {
        const string sql = """
            INSERT INTO Vacunas (IdMascota, NombreVacuna, FechaAplicacion, ProximaDosis, Lote)
            VALUES (@IdMascota, @NombreVacuna, @FechaAplicacion, @ProximaDosis, @Lote);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdMascota", vacuna.IdMascota);
        comando.Parameters.AddWithValue("@NombreVacuna", vacuna.NombreVacuna.Trim());
        comando.Parameters.AddWithValue("@FechaAplicacion", vacuna.FechaAplicacion.Date);
        comando.Parameters.AddWithValue("@ProximaDosis", vacuna.ProximaDosis.HasValue ? vacuna.ProximaDosis.Value.Date : DBNull.Value);
        comando.Parameters.AddWithValue("@Lote", vacuna.Lote.Trim());
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }
}

