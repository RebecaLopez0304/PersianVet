using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas asíncronas usadas por los reportes.
/// </summary>
public interface IReporteDatos
{
    /// <summary>
    /// Resume las citas, consultas, tratamientos e ingresos en C$ de cada veterinario activo en el período.
    /// </summary>
    /// <param name="desde">Primer día del período.</param>
    /// <param name="hasta">Último día del período (incluido).</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Una fila por veterinario.</returns>
    Task<List<ReporteVeterinario>> ObtenerActividadPorVeterinarioAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion);

    /// <summary>
    /// Resume las mascotas activas, consultas, vacunas e ingresos en C$ de cada especie en el período.
    /// </summary>
    /// <param name="desde">Primer día del período.</param>
    /// <param name="hasta">Último día del período (incluido).</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Una fila por especie.</returns>
    Task<List<ReporteEspecie>> ObtenerAtencionPorEspecieAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion);
}
