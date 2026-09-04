using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de clientes en SQL Server.
/// </summary>
public class ClienteDatos : IClienteDatos
{
    private readonly ConexionBD conexionBD;

    public ClienteDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Cliente> Listar(string filtro, bool soloActivos = true)
    {
        const string sql = """
            SELECT IdCliente, Nombres, Apellidos, Telefono, Direccion, Correo, FechaRegistro, Activo
            FROM Clientes
            WHERE (@SoloActivos = 0 OR Activo = 1)
              AND (@Filtro = ''
               OR Nombres LIKE '%' + @Filtro + '%'
               OR Apellidos LIKE '%' + @Filtro + '%'
               OR Telefono LIKE '%' + @Filtro + '%'
               OR Correo LIKE '%' + @Filtro + '%')
            ORDER BY IdCliente DESC;
            """;

        List<Cliente> clientes = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Filtro", filtro.Trim());
        comando.Parameters.AddWithValue("@SoloActivos", soloActivos);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            clientes.Add(LeerCliente(lector));
        }

        return clientes;
    }

    public int Guardar(Cliente cliente)
    {
        const string sql = """
            INSERT INTO Clientes (Nombres, Apellidos, Telefono, Direccion, Correo, FechaRegistro, Activo)
            VALUES (@Nombres, @Apellidos, @Telefono, @Direccion, @Correo, @FechaRegistro, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, cliente);
        conexion.Open();
        return (int)comando.ExecuteScalar()!;
    }

    public void Actualizar(Cliente cliente)
    {
        const string sql = """
            UPDATE Clientes
            SET Nombres = @Nombres, Apellidos = @Apellidos, Telefono = @Telefono,
                Direccion = @Direccion, Correo = @Correo, FechaRegistro = @FechaRegistro
            WHERE IdCliente = @IdCliente;
            """;

        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = CrearComando(sql, conexion, cliente);
        comando.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public void Desactivar(int idCliente)
    {
        const string sql = "UPDATE Clientes SET Activo = 0 WHERE IdCliente = @IdCliente;";
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@IdCliente", idCliente);
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    private static SqlCommand CrearComando(string sql, SqlConnection conexion, Cliente cliente)
    {
        SqlCommand comando = new(sql, conexion);
        comando.Parameters.AddWithValue("@Nombres", cliente.Nombres.Trim());
        comando.Parameters.AddWithValue("@Apellidos", cliente.Apellidos.Trim());
        comando.Parameters.AddWithValue("@Telefono", cliente.Telefono.Trim());
        comando.Parameters.AddWithValue("@Direccion", cliente.Direccion.Trim());
        comando.Parameters.AddWithValue("@Correo", cliente.Correo.Trim());
        comando.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro.Date);
        return comando;
    }

    private static Cliente LeerCliente(SqlDataReader lector)
    {
        return new Cliente
        {
            IdCliente = lector.GetInt32(0),
            Nombres = lector.GetString(1),
            Apellidos = lector.GetString(2),
            Telefono = lector.GetString(3),
            Direccion = lector.GetString(4),
            Correo = lector.GetString(5),
            FechaRegistro = lector.GetDateTime(6),
            Activo = lector.GetBoolean(7)
        };
    }
}
