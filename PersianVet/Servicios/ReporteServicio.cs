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
    /// Genera en paralelo los reportes por veterinario y por especie del período indicado.
    /// </summary>
    /// <param name="desde">Primer día del período.</param>
    /// <param name="hasta">Último día del período (incluido).</param>
    /// <param name="cancelacion">Permite cancelar la generación.</param>
    /// <returns>Las filas de ambos reportes.</returns>
    /// <exception cref="ArgumentException">La fecha final es anterior a la inicial.</exception>
    public async Task<(List<ReporteVeterinario> Veterinarios, List<ReporteEspecie> Especies)> GenerarAsync(
        DateTime desde, DateTime hasta, CancellationToken cancelacion)
    {
        if (hasta.Date < desde.Date)
        {
            throw new ArgumentException(
                $"Hasta: {hasta:dd/MM/yyyy} es anterior a Desde ({desde:dd/MM/yyyy}); elija un período válido.");
        }

        Task<List<ReporteVeterinario>> tareaVeterinarios =
            reporteDatos.ObtenerActividadPorVeterinarioAsync(desde.Date, hasta.Date, cancelacion);
        Task<List<ReporteEspecie>> tareaEspecies =
            reporteDatos.ObtenerAtencionPorEspecieAsync(desde.Date, hasta.Date, cancelacion);

        await Task.WhenAll(tareaVeterinarios, tareaEspecies);
        return (tareaVeterinarios.Result, tareaEspecies.Result);
    }
}
