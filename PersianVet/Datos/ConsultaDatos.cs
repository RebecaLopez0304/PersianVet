using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
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

        // Los tratamientos se concatenan en memoria porque string.Join no se traduce a SQL.
        var filas = contexto.Consultas.AsNoTracking()
            .Where(c => c.IdMascota == idMascota)
            .OrderByDescending(c => c.FechaHora)
            .Select(c => new
            {
                c.IdConsulta,
                c.FechaHora,
                c.Motivo,
                c.Diagnostico,
                c.Observaciones,
                Veterinario = c.Veterinario.Nombres + " " + c.Veterinario.Apellidos,
                Tratamientos = c.Tratamientos.Select(t => t.Medicamento + " - " + t.Dosis).ToList()
            })
            .ToList();

        return filas.Select(f => new Consulta
        {
            IdConsulta = f.IdConsulta,
            IdMascota = idMascota,
            FechaHora = f.FechaHora,
            Motivo = f.Motivo,
            Diagnostico = f.Diagnostico,
            Observaciones = f.Observaciones ?? string.Empty,
            Veterinario = f.Veterinario,
            Tratamientos = string.Join("; ", f.Tratamientos)
        }).ToList();
    }

    public int GuardarConTratamientos(Consulta consulta)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        ConsultaEntidad entidad = new()
        {
            IdMascota = consulta.IdMascota,
            IdVeterinario = consulta.IdVeterinario,
            FechaHora = consulta.FechaHora,
            Motivo = consulta.Motivo.Trim(),
            Diagnostico = consulta.Diagnostico.Trim(),
            Observaciones = consulta.Observaciones.Trim(),
            Tratamientos = consulta.ListaTratamientos.Select(t => new TratamientoEntidad
            {
                Medicamento = t.Medicamento.Trim(),
                Dosis = t.Dosis.Trim(),
                Frecuencia = t.Frecuencia.Trim(),
                Duracion = t.Duracion.Trim(),
                Indicaciones = t.Indicaciones.Trim()
            }).ToList()
        };

        // SaveChanges ejecuta consulta y tratamientos en una sola transacción.
        contexto.Consultas.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdConsulta;
    }
}
