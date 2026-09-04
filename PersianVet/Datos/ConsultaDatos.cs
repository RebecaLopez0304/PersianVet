using System.Data;
using Microsoft.Data.SqlClient;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Guarda consultas y tratamientos en SQL Server.
/// </summary>
public class ConsultaDatos : IConsultaDatos
{
    private readonly ConexionBD conexionBD;

    public ConsultaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Consulta> ListarPorMascota(int idMascota)
    {
        List<Consulta> consultas = [];
        using SqlConnection conexion = conexionBD.CrearConexion();
        using SqlCommand comando = new("sp_HistorialMascota", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.AddWithValue("@IdMascota", idMascota);
        conexion.Open();

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            consultas.Add(new Consulta
            {
                IdConsulta = lector.GetInt32(0),
                FechaHora = lector.GetDateTime(1),
                Motivo = lector.GetString(2),
                Diagnostico = lector.GetString(3),
                Observaciones = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                Veterinario = lector.GetString(5),
                Tratamientos = lector.IsDBNull(6) ? string.Empty : lector.GetString(6)
            });
        }

        return consultas;
    }

    public int GuardarConTratamientos(Consulta consulta)
    {
        using SqlConnection conexion = conexionBD.CrearConexion();
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();

        try
        {
            const string sqlConsulta = """
                INSERT INTO Consultas
                    (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
                VALUES
                    (@IdMascota, @IdVeterinario, @FechaHora, @Motivo, @Diagnostico, @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            using SqlCommand comandoConsulta = new(sqlConsulta, conexion, transaccion);
            comandoConsulta.Parameters.AddWithValue("@IdMascota", consulta.IdMascota);
            comandoConsulta.Parameters.AddWithValue("@IdVeterinario", consulta.IdVeterinario);
            comandoConsulta.Parameters.AddWithValue("@FechaHora", consulta.FechaHora);
            comandoConsulta.Parameters.AddWithValue("@Motivo", consulta.Motivo.Trim());
            comandoConsulta.Parameters.AddWithValue("@Diagnostico", consulta.Diagnostico.Trim());
            comandoConsulta.Parameters.AddWithValue("@Observaciones", consulta.Observaciones.Trim());
            int idConsulta = (int)comandoConsulta.ExecuteScalar()!;

            foreach (Tratamiento tratamiento in consulta.ListaTratamientos)
            {
                GuardarTratamiento(tratamiento, idConsulta, conexion, transaccion);
            }

            transaccion.Commit();
            return idConsulta;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    private static void GuardarTratamiento(
        Tratamiento tratamiento,
        int idConsulta,
        SqlConnection conexion,
        SqlTransaction transaccion)
    {
        const string sql = """
            INSERT INTO Tratamientos
                (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
            VALUES
                (@IdConsulta, @Medicamento, @Dosis, @Frecuencia, @Duracion, @Indicaciones);
            """;

        using SqlCommand comando = new(sql, conexion, transaccion);
        comando.Parameters.AddWithValue("@IdConsulta", idConsulta);
        comando.Parameters.AddWithValue("@Medicamento", tratamiento.Medicamento.Trim());
        comando.Parameters.AddWithValue("@Dosis", tratamiento.Dosis.Trim());
        comando.Parameters.AddWithValue("@Frecuencia", tratamiento.Frecuencia.Trim());
        comando.Parameters.AddWithValue("@Duracion", tratamiento.Duracion.Trim());
        comando.Parameters.AddWithValue("@Indicaciones", tratamiento.Indicaciones.Trim());
        comando.ExecuteNonQuery();
    }
}

