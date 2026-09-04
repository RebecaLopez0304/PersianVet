using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de citas en SQL Server.
/// </summary>
public class CitaDatos : ICitaDatos
{
    private readonly ConexionBD conexionBD;

    public CitaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Cita> Listar(DateTime? fecha = null)
    {
        const string sql = """
            SELECT ci.IdCita, ci.IdMascota, ci.IdVeterinario, ci.FechaHora,
                   ci.Motivo, ci.Estado, m.Nombre AS Mascota,
                   c.Nombres + ' ' + c.Apellidos AS Cliente,
                   v.Nombres + ' ' + v.Apellidos AS Veterinario
            FROM Citas ci
            INNER JOIN Mascotas m ON m.IdMascota = ci.IdMascota
            INNER JOIN Clientes c ON c.IdCliente = m.IdCliente
            INNER JOIN Veterinarios v ON v.IdVeterinario = ci.IdVeterinario
            WHERE @Fecha IS NULL OR CAST(ci.FechaHora AS DATE) = @Fecha
            ORDER BY ci.FechaHora;
            """;

        List<Cita> citas = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Fecha", fecha.HasValue ? fecha.Value.Date : DBNull.Value);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            citas.Add(LeerCita(lector));
        }

        return citas;
    }

    public bool ExisteCruce(Cita cita)
    {
        const string sql = """
            SELECT COUNT(*)
            FROM Citas
            WHERE IdVeterinario = @IdVeterinario
              AND FechaHora = @FechaHora
              AND Estado <> 'Cancelada'
              AND IdCita <> @IdCita;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdVeterinario", cita.IdVeterinario);
        comando.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
        comando.Parameters.AddWithValue("@IdCita", cita.IdCita);
        conexion.Open();
        return (int)comando.ExecuteScalar()! > 0;
    }

    public int Guardar(Cita cita)
    {
        const string sql = """
            INSERT INTO Citas (IdMascota, IdVeterinario, FechaHora, Motivo, Estado)
            VALUES (@IdMascota, @IdVeterinario, @FechaHora, @Motivo, @Estado);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, cita);
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }

    public void Actualizar(Cita cita)
    {
        const string sql = """
            UPDATE Citas
            SET IdMascota = @IdMascota, IdVeterinario = @IdVeterinario,
                FechaHora = @FechaHora, Motivo = @Motivo, Estado = @Estado
            WHERE IdCita = @IdCita;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, cita);
        comando.Parameters.AddWithValue("@IdCita", cita.IdCita);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Cancelar(int idCita)
    {
        const string sql = "UPDATE Citas SET Estado = 'Cancelada' WHERE IdCita = @IdCita;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdCita", idCita);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static SqlCommand CrearComando(string sql, SqlConnection conexion, Cita cita)
    {
        SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdMascota", cita.IdMascota);
        comando.Parameters.AddWithValue("@IdVeterinario", cita.IdVeterinario);
        comando.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
        comando.Parameters.AddWithValue("@Motivo", cita.Motivo.Trim());
        comando.Parameters.AddWithValue("@Estado", cita.Estado);
        return comando;
    }

    private static Cita LeerCita(SqlDataReader lector)
    {
        return new Cita
        {
            IdCita = lector.GetInt32(0),
            IdMascota = lector.GetInt32(1),
            IdVeterinario = lector.GetInt32(2),
            FechaHora = lector.GetDateTime(3),
            Motivo = lector.GetString(4),
            Estado = lector.GetString(5),
            Mascota = lector.GetString(6),
            Cliente = lector.GetString(7),
            Veterinario = lector.GetString(8)
        };
    }
}

