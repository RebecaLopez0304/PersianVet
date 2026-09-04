using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de mascotas en SQL Server.
/// </summary>
public class MascotaDatos : IMascotaDatos
{
    private readonly ConexionBD conexionBD;

    public MascotaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Mascota> Listar(string filtro, bool soloActivas = true)
    {
        const string sql = """
            SELECT m.IdMascota, m.IdCliente, c.Nombres + ' ' + c.Apellidos AS Propietario,
                   m.Nombre, m.Especie, m.Raza, m.Sexo, m.FechaNacimiento,
                   m.Color, m.Peso, m.Observaciones, m.Activa
            FROM Mascotas m
            INNER JOIN Clientes c ON c.IdCliente = m.IdCliente
            WHERE (@SoloActivas = 0 OR m.Activa = 1)
              AND (@Filtro = '' OR m.Nombre LIKE '%' + @Filtro + '%'
                   OR c.Nombres LIKE '%' + @Filtro + '%'
                   OR c.Apellidos LIKE '%' + @Filtro + '%')
            ORDER BY m.IdMascota DESC;
            """;

        List<Mascota> mascotas = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Filtro", filtro.Trim());
        comando.Parameters.AddWithValue("@SoloActivas", soloActivas);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            mascotas.Add(LeerMascota(lector));
        }

        return mascotas;
    }

    public int Guardar(Mascota mascota)
    {
        const string sql = """
            INSERT INTO Mascotas
                (IdCliente, Nombre, Especie, Raza, Sexo, FechaNacimiento, Color, Peso, Observaciones, Activa)
            VALUES
                (@IdCliente, @Nombre, @Especie, @Raza, @Sexo, @FechaNacimiento, @Color, @Peso, @Observaciones, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, mascota);
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }

    public void Actualizar(Mascota mascota)
    {
        const string sql = """
            UPDATE Mascotas
            SET IdCliente = @IdCliente, Nombre = @Nombre, Especie = @Especie,
                Raza = @Raza, Sexo = @Sexo, FechaNacimiento = @FechaNacimiento,
                Color = @Color, Peso = @Peso, Observaciones = @Observaciones
            WHERE IdMascota = @IdMascota;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, mascota);
        comando.Parameters.AddWithValue("@IdMascota", mascota.IdMascota);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Desactivar(int idMascota)
    {
        const string sql = "UPDATE Mascotas SET Activa = 0 WHERE IdMascota = @IdMascota;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdMascota", idMascota);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static SqlCommand CrearComando(string sql, SqlConnection conexion, Mascota mascota)
    {
        SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdCliente", mascota.IdCliente);
        comando.Parameters.AddWithValue("@Nombre", mascota.Nombre.Trim());
        comando.Parameters.AddWithValue("@Especie", mascota.Especie.Trim());
        comando.Parameters.AddWithValue("@Raza", mascota.Raza.Trim());
        comando.Parameters.AddWithValue("@Sexo", mascota.Sexo);
        comando.Parameters.AddWithValue("@FechaNacimiento", mascota.FechaNacimiento.Date);
        comando.Parameters.AddWithValue("@Color", mascota.Color.Trim());
        comando.Parameters.AddWithValue("@Peso", mascota.Peso);
        comando.Parameters.AddWithValue("@Observaciones", mascota.Observaciones.Trim());
        return comando;
    }

    private static Mascota LeerMascota(SqlDataReader lector)
    {
        return new Mascota
        {
            IdMascota = lector.GetInt32(0),
            IdCliente = lector.GetInt32(1),
            Propietario = lector.GetString(2),
            Nombre = lector.GetString(3),
            Especie = lector.GetString(4),
            Raza = lector.GetString(5),
            Sexo = lector.GetString(6),
            FechaNacimiento = lector.GetDateTime(7),
            Color = lector.GetString(8),
            Peso = lector.GetDecimal(9),
            Observaciones = lector.IsDBNull(10) ? string.Empty : lector.GetString(10),
            Activa = lector.GetBoolean(11)
        };
    }
}
