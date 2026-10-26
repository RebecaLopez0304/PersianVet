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

    /// <inheritdoc cref="IDashboardDatos.ObtenerResumenAsync(CancellationToken)"/>
    public Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ObtenerResumenAsync(cancelacion);
    }

    /// <inheritdoc cref="IDashboardDatos.ListarProximasCitasAsync(CancellationToken)"/>
    public Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ListarProximasCitasAsync(cancelacion);
    }

    /// <inheritdoc cref="IDashboardDatos.ListarAlertasAsync(CancellationToken)"/>
    public Task<List<Alerta>> ListarAlertasAsync(CancellationToken cancelacion = default)
    {
        return dashboardDatos.ListarAlertasAsync(cancelacion);
    }

    /// <inheritdoc cref="IDashboardDatos.ContarCitasProximosDiasAsync(int, CancellationToken)"/>
    public Task<List<PuntoGrafico>> ContarCitasProximosDiasAsync(int dias, CancellationToken cancelacion = default)
    {
        return dashboardDatos.ContarCitasProximosDiasAsync(dias, cancelacion);
    }
}
