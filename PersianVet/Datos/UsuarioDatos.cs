using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Consulta los usuarios guardados en la base de datos.
/// </summary>
public class UsuarioDatos : IUsuarioDatos
{
    private readonly ConexionBD conexionBD;

    public UsuarioDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public Usuario? ObtenerPorNombre(string nombreUsuario)
    {
        const string sql = """
            SELECT IdUsuario, NombreUsuario, ClaveHash, Rol, Activo
            FROM Usuarios
            WHERE NombreUsuario = @NombreUsuario;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        if (!lector.Read())
        {
            return null;
        }

        return new Usuario
        {
            IdUsuario = lector.GetInt32(0),
            NombreUsuario = lector.GetString(1),
            ClaveHash = lector.GetString(2),
            Rol = lector.GetString(3),
            Activo = lector.GetBoolean(4)
        };
    }

    public bool Existe(string nombreUsuario)
    {
        const string sql = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim());
        conexion.Open();
        return (int)comando.ExecuteScalar()! > 0;
    }

    public int Crear(Usuario usuario)
    {
        const string sql = """
            INSERT INTO Usuarios (NombreUsuario, ClaveHash, Rol, Activo)
            VALUES (@NombreUsuario, @ClaveHash, @Rol, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario.Trim());
        comando.Parameters.AddWithValue("@ClaveHash", usuario.ClaveHash);
        comando.Parameters.AddWithValue("@Rol", usuario.Rol);
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }

    public void CambiarClave(string nombreUsuario, string claveHash)
    {
        const string sql = """
            UPDATE Usuarios
            SET ClaveHash = @ClaveHash
            WHERE NombreUsuario = @NombreUsuario;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario.Trim());
        comando.Parameters.AddWithValue("@ClaveHash", claveHash);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public List<Usuario> Listar(bool incluirDesactivados = false)
    {
        const string sql = """
            SELECT IdUsuario, NombreUsuario, ClaveHash, Rol, Activo
            FROM Usuarios
            WHERE @IncluirDesactivados = 1 OR Activo = 1
            ORDER BY NombreUsuario;
            """;
        List<Usuario> usuarios = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IncluirDesactivados", incluirDesactivados);
        conexion.Open();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            usuarios.Add(new Usuario
            {
                IdUsuario = lector.GetInt32(0), NombreUsuario = lector.GetString(1),
                ClaveHash = lector.GetString(2), Rol = lector.GetString(3), Activo = lector.GetBoolean(4)
            });
        }
        return usuarios;
    }

    public void Desactivar(int idUsuario)
    {
        const string sql = "UPDATE Usuarios SET Activo = 0 WHERE IdUsuario = @IdUsuario;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
        conexion.Open();
        comando.ExecuteNonQuery();
    }
}
