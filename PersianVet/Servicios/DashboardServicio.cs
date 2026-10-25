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

    public Task<List<Alerta>> ListarAlertasAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ListarAlertasAsync(cancelacion);
    }

    public Task<List<PuntoGrafico>> ContarCitasProximosDiasAsync(int dias, CancellationToken cancelacion = default)
    {
        return dashboardDatos.ContarCitasProximosDiasAsync(dias, cancelacion);
    }
}
