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

    /// <inheritdoc/>
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
                    .SelectMany(c => c.ListaTratamientos)
                    .Count(),
                // SQL Server no admite un SUM dentro de otro: consultas y tratamientos se suman por separado.
                Ingresos = v.Consultas
                    .Where(c => c.FechaHora >= desde && c.FechaHora < fin)
                    .Sum(c => c.Precio)
                    + v.Consultas
                    .Where(c => c.FechaHora >= desde && c.FechaHora < fin)
                    .SelectMany(c => c.ListaTratamientos)
                    .Sum(t => t.Precio)
            })
            .OrderByDescending(r => r.Consultas)
            .ThenBy(r => r.Veterinario)
            .ToListAsync(cancelacion);
    }

    /// <inheritdoc/>
    public async Task<List<ReporteEspecie>> ObtenerAtencionPorEspecieAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        DateTime fin = hasta.Date.AddDays(1);
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        // Se incluyen las mascotas desactivadas para no perder lo que se les cobró en el período.
        var porMascota = await contexto.Mascotas.AsNoTracking()
            .Select(m => new
            {
                m.Especie,
                m.Activa,
                Consultas = m.Consultas.Count(c => c.FechaHora >= desde && c.FechaHora < fin),
                Vacunas = m.Vacunas.Count(v => v.FechaAplicacion >= desde && v.FechaAplicacion < fin),
                Ingresos = m.Consultas
                    .Where(c => c.FechaHora >= desde && c.FechaHora < fin)
                    .Sum(c => c.Precio)
                    + m.Consultas
                    .Where(c => c.FechaHora >= desde && c.FechaHora < fin)
                    .SelectMany(c => c.ListaTratamientos)
                    .Sum(t => t.Precio)
                    + m.Vacunas
                    .Where(v => v.FechaAplicacion >= desde && v.FechaAplicacion < fin)
                    .Sum(v => v.Precio)
            })
            .ToListAsync(cancelacion);

        return porMascota
            .GroupBy(m => m.Especie)
            .Select(g => new ReporteEspecie
            {
                Especie = g.Key,
                Mascotas = g.Count(m => m.Activa),
                Consultas = g.Sum(m => m.Consultas),
                Vacunas = g.Sum(m => m.Vacunas),
                Ingresos = g.Sum(m => m.Ingresos)
            })
            .OrderByDescending(r => r.Mascotas)
            .ThenBy(r => r.Especie)
            .ToList();
    }
}
