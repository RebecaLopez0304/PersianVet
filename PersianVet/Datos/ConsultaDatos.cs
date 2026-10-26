using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Guarda consultas y tratamientos mediante Entity Framework Core.
/// </summary>
public class ConsultaDatos : IConsultaDatos
{
    private readonly ConexionBD conexionBD;

    public ConsultaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public List<Consulta> ListarPorMascota(int idMascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include carga el veterinario y los tratamientos que se muestran en el historial.
        return contexto.Consultas.AsNoTracking()
            .Include(c => c.Veterinario)
            .Include(c => c.ListaTratamientos)
            .Where(c => c.IdMascota == idMascota)
            .OrderByDescending(c => c.FechaHora)
            .ToList();
    }

    /// <inheritdoc/>
    public int GuardarConTratamientos(Consulta consulta)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        // Sin las relaciones cargadas, EF inserta solo la consulta y sus tratamientos.
        consulta.Mascota = null!;
        consulta.Veterinario = null!;
        consulta.Motivo = consulta.Motivo.Trim();
        consulta.Diagnostico = consulta.Diagnostico.Trim();
        consulta.Observaciones = (consulta.Observaciones ?? string.Empty).Trim();
        foreach (Tratamiento tratamiento in consulta.ListaTratamientos)
        {
            tratamiento.Medicamento = tratamiento.Medicamento.Trim();
            tratamiento.Dosis = tratamiento.Dosis.Trim();
            tratamiento.Frecuencia = tratamiento.Frecuencia.Trim();
            tratamiento.Duracion = tratamiento.Duracion.Trim();
            tratamiento.Indicaciones = (tratamiento.Indicaciones ?? string.Empty).Trim();
        }

        // SaveChanges guarda la consulta y sus tratamientos en una sola transacción.
        contexto.Consultas.Add(consulta);
        contexto.SaveChanges();
        return consulta.IdConsulta;
    }
}
