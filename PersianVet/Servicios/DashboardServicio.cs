using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Obtiene la información mostrada en el inicio.
/// </summary>
public class DashboardServicio
{
    private readonly IDashboardDatos dashboardDatos;

    public DashboardServicio(IDashboardDatos dashboardDatos)
    {
        this.dashboardDatos = dashboardDatos;
    }

    public Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ObtenerResumenAsync(cancelacion);
    }

    public Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ListarProximasCitasAsync(cancelacion);
    }
}
