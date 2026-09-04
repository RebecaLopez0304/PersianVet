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

    public ResumenDashboard ObtenerResumen()
    {
        return dashboardDatos.ObtenerResumen();
    }

    public List<Cita> ListarProximasCitas()
    {
        return dashboardDatos.ListarProximasCitas();
    }
}
