using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las consultas y sus tratamientos.
/// </summary>
public class ConsultaServicio
{
    private readonly IConsultaDatos consultaDatos;

    public ConsultaServicio(IConsultaDatos consultaDatos)
    {
        this.consultaDatos = consultaDatos;
    }

    /// <inheritdoc cref="IConsultaDatos.ListarPorMascota(int)"/>
    public List<Consulta> ListarPorMascota(int idMascota)
    {
        return idMascota > 0 ? consultaDatos.ListarPorMascota(idMascota) : [];
    }

    /// <summary>
    /// Valida la consulta (sin fecha futura) y que cada tratamiento tenga medicamento y dosis, y la registra con sus tratamientos.
    /// </summary>
    /// <param name="consulta">Consulta con la lista de tratamientos.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Consulta consulta)
    {
        if (consulta.IdMascota <= 0 || consulta.IdVeterinario <= 0)
        {
            return ResultadoOperacion.Error("Seleccione la mascota y el veterinario.");
        }

        if (consulta.FechaHora > DateTime.Now.AddMinutes(1))
        {
            return ResultadoOperacion.Error("La consulta no puede tener una fecha futura.");
        }

        if (string.IsNullOrWhiteSpace(consulta.Motivo) || string.IsNullOrWhiteSpace(consulta.Diagnostico))
        {
            return ResultadoOperacion.Error("Escriba el motivo y el diagnóstico.");
        }

        foreach (Tratamiento tratamiento in consulta.ListaTratamientos)
        {
            if (string.IsNullOrWhiteSpace(tratamiento.Medicamento) || string.IsNullOrWhiteSpace(tratamiento.Dosis))
            {
                return ResultadoOperacion.Error("Cada tratamiento necesita medicamento y dosis.");
            }
        }

        consulta.IdConsulta = consultaDatos.GuardarConTratamientos(consulta);
        return ResultadoOperacion.Correcto("La consulta se guardó correctamente.");
    }
}

