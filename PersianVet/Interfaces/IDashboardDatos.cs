using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas usadas por el dashboard.
/// </summary>
public interface IDashboardDatos
{
    ResumenDashboard ObtenerResumen();
    List<Cita> ListarProximasCitas();
}

