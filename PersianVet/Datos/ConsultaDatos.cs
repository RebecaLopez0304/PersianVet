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

    public List<Consulta> ListarPorMascota(int idMascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include trae el veterinario y los tratamientos de cada consulta.
        return contexto.Consultas.AsNoTracking()
            .Include(c => c.Veterinario)
            .Include(c => c.ListaTratamientos)
            .Where(c => c.IdMascota == idMascota)
            .OrderByDescending(c => c.FechaHora)
            .ToList();
    }

    public int GuardarConTratamientos(Consulta consulta)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
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
