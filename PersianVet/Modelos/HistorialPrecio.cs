using System.ComponentModel;
using PersianVet.Utilidades;

namespace PersianVet.Modelos;

/// <summary>
/// Un cambio de precio de una tarifa (tabla HistorialPrecios).
/// </summary>
public class HistorialPrecio
{
    public int IdHistorial { get; set; }
    public int IdTarifa { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    /// <summary>
    /// Obtiene o establece el precio que tenía antes del cambio, o <see langword="null"/> si es el precio inicial.
    /// </summary>
    [DisplayName("Precio anterior")]
    public decimal? PrecioAnterior { get; set; }

    [DisplayName("Precio nuevo")]
    public decimal PrecioNuevo { get; set; }

    /// <summary>
    /// Obtiene la variación en texto, por ejemplo "+C$50.00 (12.5 %)" o "Precio inicial".
    /// </summary>
    public string Cambio
    {
        get
        {
            if (PrecioAnterior is not decimal anterior)
            {
                return "Precio inicial";
            }

            decimal diferencia = PrecioNuevo - anterior;
            string signo = diferencia >= 0 ? "+" : "-";
            string porcentaje = anterior == 0 ? string.Empty : $" ({signo}{Math.Abs(diferencia) / anterior * 100:0.#} %)";
            return $"{signo}{Moneda.Formatear(Math.Abs(diferencia))}{porcentaje}";
        }
    }

    public string Usuario { get; set; } = string.Empty;
}
