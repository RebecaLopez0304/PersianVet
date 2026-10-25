using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas usadas por el dashboard.
/// </summary>
public interface IDashboardDatos
{
    Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion);
    Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion);
    Task<List<Alerta>> ListarAlertasAsync(CancellationToken cancelacion);
    Task<List<PuntoGrafico>> ContarCitasProximosDiasAsync(int dias, CancellationToken cancelacion);
}
