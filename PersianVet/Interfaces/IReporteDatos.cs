using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas asíncronas usadas por los reportes.
/// </summary>
public interface IReporteDatos
{
    Task<List<ReporteVeterinario>> ObtenerActividadPorVeterinarioAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion);

    Task<List<ReporteEspecie>> ObtenerAtencionPorEspecieAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion);
}
