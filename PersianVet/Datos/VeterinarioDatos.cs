using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de veterinarios en SQL Server.
/// </summary>
public class VeterinarioDatos : IVeterinarioDatos
{
    private readonly ConexionBD conexionBD;

    public VeterinarioDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Veterinario> Listar(string filtro, bool soloActivos = true)
    {
        const string sql = """
            SELECT IdVeterinario, Nombres, Apellidos, Especialidad, Telefono, Activo
            FROM Veterinarios
            WHERE (@SoloActivos = 0 OR Activo = 1)
              AND (@Filtro = '' OR Nombres LIKE '%' + @Filtro + '%'
                   OR Apellidos LIKE '%' + @Filtro + '%'
                   OR Especialidad LIKE '%' + @Filtro + '%')
            ORDER BY IdVeterinario DESC;
            """;

        List<Veterinario> veterinarios = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Filtro", filtro.Trim());
        comando.Parameters.AddWithValue("@SoloActivos", soloActivos);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            veterinarios.Add(new Veterinario
            {
                IdVeterinario = lector.GetInt32(0),
                Nombres = lector.GetString(1),
                Apellidos = lector.GetString(2),
                Especialidad = lector.GetString(3),
                Telefono = lector.GetString(4),
                Activo = lector.GetBoolean(5)
            });
        }

        return veterinarios;
    }

    public int Guardar(Veterinario veterinario)
    {
        const string sql = """
            INSERT INTO Veterinarios (Nombres, Apellidos, Especialidad, Telefono, Activo)
            VALUES (@Nombres, @Apellidos, @Especialidad, @Telefono, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, veterinario);
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }

    public void Actualizar(Veterinario veterinario)
    {
        const string sql = """
            UPDATE Veterinarios
            SET Nombres = @Nombres, Apellidos = @Apellidos,
                Especialidad = @Especialidad, Telefono = @Telefono
            WHERE IdVeterinario = @IdVeterinario;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, veterinario);
        comando.Parameters.AddWithValue("@IdVeterinario", veterinario.IdVeterinario);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Desactivar(int idVeterinario)
    {
        const string sql = "UPDATE Veterinarios SET Activo = 0 WHERE IdVeterinario = @IdVeterinario;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdVeterinario", idVeterinario);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static SqlCommand CrearComando(string sql, SqlConnection conexion, Veterinario veterinario)
    {
        SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Nombres", veterinario.Nombres.Trim());
        comando.Parameters.AddWithValue("@Apellidos", veterinario.Apellidos.Trim());
        comando.Parameters.AddWithValue("@Especialidad", veterinario.Especialidad.Trim());
        comando.Parameters.AddWithValue("@Telefono", veterinario.Telefono.Trim());
        return comando;
    }
}
