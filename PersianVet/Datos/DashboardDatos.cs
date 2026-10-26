using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Consulta con Entity Framework Core la información mostrada en el dashboard.
/// </summary>
public class DashboardDatos : IDashboardDatos
{
    private readonly ConexionBD conexionBD;

    public DashboardDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public async Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        DateTime manana = hoy.AddDays(1);
        DateTime limiteVacunas = hoy.AddDays(30);
        DateTime inicioMes = new(hoy.Year, hoy.Month, 1);
        DateTime inicioMesSiguiente = inicioMes.AddMonths(1);

        // DbContext no admite consultas simultáneas: cada conteo paralelo usa su propio contexto.
        Task<int> mascotas = ConsultarAsync(c => c.Mascotas.CountAsync(m => m.Activa, cancelacion));
        Task<int> citas = ConsultarAsync(c => c.Citas.CountAsync(
            ci => ci.FechaHora >= hoy && ci.FechaHora < manana && ci.Estado != "Cancelada", cancelacion));
        Task<int> vacunas = ConsultarAsync(c => c.Vacunas.CountAsync(
            v => v.ProximaDosis >= hoy && v.ProximaDosis <= limiteVacunas, cancelacion));
        Task<int> consultas = ConsultarAsync(c => c.Consultas.CountAsync(
            co => co.FechaHora >= inicioMes && co.FechaHora < inicioMesSiguiente, cancelacion));
        Task<decimal> ingresos = ConsultarAsync(c => SumarIngresosAsync(c, inicioMes, inicioMesSiguiente, cancelacion));

        await Task.WhenAll(mascotas, citas, vacunas, consultas, ingresos);

        return new ResumenDashboard
        {
            MascotasRegistradas = mascotas.Result,
            CitasHoy = citas.Result,
            VacunasProximas = vacunas.Result,
            ConsultasMes = consultas.Result,
            IngresosMes = ingresos.Result
        };
    }

    /// <inheritdoc/>
    public async Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion)
    {
        DateTime ahora = DateTime.Now;
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        return await contexto.Citas.AsNoTracking()
            .Include(c => c.Mascota).ThenInclude(m => m.Cliente)
            .Include(c => c.Veterinario)
            .Where(c => c.FechaHora >= ahora && c.Estado != "Cancelada")
            .OrderBy(c => c.FechaHora)
            .Take(8)
            .ToListAsync(cancelacion);
    }

    /// <inheritdoc/>
    public async Task<List<Alerta>> ListarAlertasAsync(CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        DateTime ahora = DateTime.Now;
        await using PersianVetContext contexto = conexionBD.CrearContexto();
        List<Alerta> alertas = [];

        // 1. Vacunas cuya próxima dosis vence en los próximos 7 días (o venció hace poco).
        var vacunas = await contexto.Vacunas.AsNoTracking()
            .Where(v => v.ProximaDosis >= hoy.AddDays(-30) && v.ProximaDosis <= hoy.AddDays(7))
            .OrderBy(v => v.ProximaDosis)
            .Take(4)
            .Select(v => new { v.NombreVacuna, Mascota = v.Mascota.Nombre, v.ProximaDosis })
            .ToListAsync(cancelacion);

        alertas.AddRange(vacunas.Select(v => new Alerta
        {
            Tipo = v.ProximaDosis < hoy ? "Vacuna vencida" : "Vacuna próxima",
            Mensaje = $"{v.NombreVacuna} de {v.Mascota} ({v.ProximaDosis:dd/MM})",
            Modulo = "Vacunas"
        }));

        // 2. Citas de las próximas 48 horas que todavía están sin confirmar.
        var citas = await contexto.Citas.AsNoTracking()
            .Where(c => c.Estado == "Programada" && c.FechaHora >= ahora && c.FechaHora <= ahora.AddDays(2))
            .OrderBy(c => c.FechaHora)
            .Take(4)
            .Select(c => new { Mascota = c.Mascota.Nombre, c.FechaHora })
            .ToListAsync(cancelacion);

        alertas.AddRange(citas.Select(c => new Alerta
        {
            Tipo = "Cita sin confirmar",
            Mensaje = $"{c.Mascota} ({c.FechaHora:dd/MM HH:mm})",
            Modulo = "Citas"
        }));

        // 3. Mascotas activas sin ninguna consulta en los últimos 6 meses.
        DateTime limite = hoy.AddMonths(-6);
        List<string> sinConsulta = await contexto.Mascotas.AsNoTracking()
            .Where(m => m.Activa && !m.Consultas.Any(c => c.FechaHora >= limite))
            .OrderBy(m => m.Nombre)
            .Take(3)
            .Select(m => m.Nombre)
            .ToListAsync(cancelacion);

        alertas.AddRange(sinConsulta.Select(nombre => new Alerta
        {
            Tipo = "Sin consulta en 6 meses",
            Mensaje = nombre,
            Modulo = "Mascotas"
        }));

        if (alertas.Count == 0)
        {
            alertas.Add(new Alerta { Tipo = "Todo en orden", Mensaje = "No hay avisos pendientes." });
        }

        return alertas;
    }

    /// <inheritdoc/>
    public async Task<List<PuntoGrafico>> ContarCitasProximosDiasAsync(int dias, CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        DateTime fin = hoy.AddDays(dias);
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        List<DateTime> fechas = await contexto.Citas.AsNoTracking()
            .Where(c => c.FechaHora >= hoy && c.FechaHora < fin && c.Estado != "Cancelada")
            .Select(c => c.FechaHora)
            .ToListAsync(cancelacion);

        // Una barra por día, incluso si ese día no tiene citas.
        return Enumerable.Range(0, dias)
            .Select(i => hoy.AddDays(i))
            .Select(dia => new PuntoGrafico
            {
                Etiqueta = dia.ToString("ddd d"),
                Valor = fechas.Count(f => f.Date == dia)
            })
            .ToList();
    }

    /// <summary>
    /// Suma lo cobrado en consultas, tratamientos y vacunas entre las dos fechas.
    /// </summary>
    private static async Task<decimal> SumarIngresosAsync(
        PersianVetContext contexto, DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        IQueryable<Consulta> consultas = contexto.Consultas.Where(c => c.FechaHora >= desde && c.FechaHora < hasta);
        decimal servicios = await consultas.SumAsync(c => c.Precio, cancelacion);
        decimal tratamientos = await consultas.SelectMany(c => c.ListaTratamientos).SumAsync(t => t.Precio, cancelacion);
        decimal vacunas = await contexto.Vacunas
            .Where(v => v.FechaAplicacion >= desde && v.FechaAplicacion < hasta)
            .SumAsync(v => v.Precio, cancelacion);
        return servicios + tratamientos + vacunas;
    }

    private async Task<T> ConsultarAsync<T>(Func<PersianVetContext, Task<T>> consulta)
    {
        await using PersianVetContext contexto = conexionBD.CrearContexto();
        return await consulta(contexto);
    }
}
