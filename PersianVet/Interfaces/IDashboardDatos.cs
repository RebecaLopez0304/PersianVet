using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas usadas por el dashboard.
/// </summary>
public interface IDashboardDatos
{
    /// <summary>
    /// Obtiene los totales de las tarjetas del dashboard.
    /// </summary>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Mascotas activas, citas de hoy, vacunas próximas, consultas e ingresos del mes.</returns>
    Task<ResumenDashboard> ObtenerResumenAsync(CancellationToken cancelacion);

    /// <summary>
    /// Obtiene las próximas ocho citas no canceladas.
    /// </summary>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Las citas ordenadas por fecha.</returns>
    Task<List<Cita>> ListarProximasCitasAsync(CancellationToken cancelacion);

    /// <summary>
    /// Obtiene los avisos del panel "Requiere atención".
    /// </summary>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Vacunas por vencer, citas sin confirmar y mascotas sin consultas recientes.</returns>
    Task<List<Alerta>> ListarAlertasAsync(CancellationToken cancelacion);

    /// <summary>
    /// Cuenta las citas no canceladas de cada uno de los próximos días.
    /// </summary>
    /// <param name="dias">Cantidad de días a partir de hoy.</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Un punto por día, incluso los días sin citas.</returns>
    Task<List<PuntoGrafico>> ContarCitasProximosDiasAsync(int dias, CancellationToken cancelacion);
}
