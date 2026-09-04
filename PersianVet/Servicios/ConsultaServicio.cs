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

    public List<Consulta> ListarPorMascota(int idMascota)
    {
        return idMascota > 0 ? consultaDatos.ListarPorMascota(idMascota) : [];
    }

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

