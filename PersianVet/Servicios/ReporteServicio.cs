using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Genera los reportes dinámicos de la clínica.
/// </summary>
public class ReporteServicio
{
    private readonly IReporteDatos reporteDatos;

    public ReporteServicio(IReporteDatos reporteDatos)
    {
        this.reporteDatos = reporteDatos;
    }

    /// <summary>
    /// Consulta en paralelo los dos reportes del período indicado.
    /// Cada consulta usa su propio contexto, por lo que pueden ejecutarse al mismo tiempo.
    /// </summary>
    public async Task<(List<ReporteVeterinario> Veterinarios, List<ReporteEspecie> Especies)> GenerarAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        if (hasta.Date < desde.Date)
        {
            throw new ArgumentException("La fecha final no puede ser anterior a la fecha inicial.");
        }

        Task<List<ReporteVeterinario>> tareaVeterinarios =
            reporteDatos.ObtenerActividadPorVeterinarioAsync(desde.Date, hasta.Date, cancelacion);
        Task<List<ReporteEspecie>> tareaEspecies =
            reporteDatos.ObtenerAtencionPorEspecieAsync(desde.Date, hasta.Date, cancelacion);

        await Task.WhenAll(tareaVeterinarios, tareaEspecies);
        return (tareaVeterinarios.Result, tareaEspecies.Result);
    }
}
