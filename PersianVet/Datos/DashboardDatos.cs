using System.Data;
using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Consulta la información mostrada en el dashboard.
/// </summary>
public class DashboardDatos : IDashboardDatos
{
    private readonly ConexionBD conexionBD;

    public DashboardDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public ResumenDashboard ObtenerResumen()
    {
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new("sp_ResumenDashboard", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        lector.Read();
        return new ResumenDashboard
        {
            MascotasRegistradas = lector.GetInt32(0),
            CitasHoy = lector.GetInt32(1),
            VacunasProximas = lector.GetInt32(2),
            ConsultasMes = lector.GetInt32(3)
        };
    }

    public List<Cita> ListarProximasCitas()
    {
        const string sql = """
            SELECT TOP 8 ci.IdCita, ci.IdMascota, ci.IdVeterinario, ci.FechaHora,
                   ci.Motivo, ci.Estado, m.Nombre,
                   c.Nombres + ' ' + c.Apellidos,
                   v.Nombres + ' ' + v.Apellidos
            FROM Citas ci
            INNER JOIN Mascotas m ON m.IdMascota = ci.IdMascota
            INNER JOIN Clientes c ON c.IdCliente = m.IdCliente
            INNER JOIN Veterinarios v ON v.IdVeterinario = ci.IdVeterinario
            WHERE ci.FechaHora >= GETDATE() AND ci.Estado <> 'Cancelada'
            ORDER BY ci.FechaHora;
            """;

        List<Cita> citas = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new(sql, conexion);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            citas.Add(new Cita
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
            });
        }

        return citas;
    }
}
