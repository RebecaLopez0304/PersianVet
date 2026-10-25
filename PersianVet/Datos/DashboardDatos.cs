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

    public async Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        DateTime manana = hoy.AddDays(1);
        DateTime limiteVacunas = hoy.AddDays(30);
        DateTime inicioMes = new(hoy.Year, hoy.Month, 1);
        DateTime inicioMesSiguiente = inicioMes.AddMonths(1);

        // Cada conteo usa su propio contexto para poder ejecutarse en paralelo.
        Task<int> mascotas = ContarAsync(c => c.Mascotas.CountAsync(m => m.Activa, cancelacion));
        Task<int> citas = ContarAsync(c => c.Citas.CountAsync(
            ci => ci.FechaHora >= hoy && ci.FechaHora < manana && ci.Estado != "Cancelada", cancelacion));
        Task<int> vacunas = ContarAsync(c => c.Vacunas.CountAsync(
            v => v.ProximaDosis >= hoy && v.ProximaDosis <= limiteVacunas, cancelacion));
        Task<int> consultas = ContarAsync(c => c.Consultas.CountAsync(
            co => co.FechaHora >= inicioMes && co.FechaHora < inicioMesSiguiente, cancelacion));

        await Task.WhenAll(mascotas, citas, vacunas, consultas);

        return new ResumenDashboard
        {
            MascotasRegistradas = mascotas.Result,
            CitasHoy = citas.Result,
            VacunasProximas = vacunas.Result,
            ConsultasMes = consultas.Result
        };
    }

    public async Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion)
    {
        DateTime ahora = DateTime.Now;
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        return await contexto.Citas.AsNoTracking()
            .Where(c => c.FechaHora >= ahora && c.Estado != "Cancelada")
            .OrderBy(c => c.FechaHora)
            .Take(8)
            .Select(c => new Cita
            {
                IdCita = c.IdCita,
                IdMascota = c.IdMascota,
                IdVeterinario = c.IdVeterinario,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                Estado = c.Estado,
                Mascota = c.Mascota.Nombre,
                Cliente = c.Mascota.Cliente.Nombres + " " + c.Mascota.Cliente.Apellidos,
                Veterinario = c.Veterinario.Nombres + " " + c.Veterinario.Apellidos
            })
            .ToListAsync(cancelacion);
    }

    private async Task<int> ContarAsync(Func<PersianVetContext, Task<int>> consulta)
    {
        await using PersianVetContext contexto = conexionBD.CrearContexto();
        return await consulta(contexto);
    }
}
