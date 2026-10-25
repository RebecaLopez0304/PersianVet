using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Consultas LINQ de agregación para los reportes, ejecutadas con Entity Framework Core.
/// </summary>
public class ReporteDatos : IReporteDatos
{
    private readonly ConexionBD conexionBD;

    public ReporteDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public async Task<List<ReporteVeterinario>> ObtenerActividadPorVeterinarioAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        DateTime fin = hasta.Date.AddDays(1);
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        return await contexto.Veterinarios.AsNoTracking()
            .Where(v => v.Activo)
            .Select(v => new ReporteVeterinario
            {
                Veterinario = v.Nombres + " " + v.Apellidos,
                Especialidad = v.Especialidad,
                Citas = v.Citas.Count(c => c.FechaHora >= desde && c.FechaHora < fin),
                Completadas = v.Citas.Count(c => c.FechaHora >= desde && c.FechaHora < fin && c.Estado == "Completada"),
                Canceladas = v.Citas.Count(c => c.FechaHora >= desde && c.FechaHora < fin && c.Estado == "Cancelada"),
                Consultas = v.Consultas.Count(c => c.FechaHora >= desde && c.FechaHora < fin),
                Tratamientos = v.Consultas
                    .Where(c => c.FechaHora >= desde && c.FechaHora < fin)
                    .SelectMany(c => c.Tratamientos)
                    .Count()
            })
            .OrderByDescending(r => r.Consultas)
            .ThenBy(r => r.Veterinario)
            .ToListAsync(cancelacion);
    }

    public async Task<List<ReporteEspecie>> ObtenerAtencionPorEspecieAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        DateTime fin = hasta.Date.AddDays(1);
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        var porMascota = await contexto.Mascotas.AsNoTracking()
            .Where(m => m.Activa)
            .Select(m => new
            {
                m.Especie,
                Consultas = m.Consultas.Count(c => c.FechaHora >= desde && c.FechaHora < fin),
                Vacunas = m.Vacunas.Count(v => v.FechaAplicacion >= desde && v.FechaAplicacion < fin)
            })
            .ToListAsync(cancelacion);

        return porMascota
            .GroupBy(m => m.Especie)
            .Select(g => new ReporteEspecie
            {
                Especie = g.Key,
                Mascotas = g.Count(),
                Consultas = g.Sum(m => m.Consultas),
                Vacunas = g.Sum(m => m.Vacunas)
            })
            .OrderByDescending(r => r.Mascotas)
            .ThenBy(r => r.Especie)
            .ToList();
    }
}
