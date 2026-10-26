using System.Globalization;

namespace PersianVet.Utilidades;

/// <summary>
/// Da formato a los montos en córdobas nicaragüenses (C$).
/// </summary>
public static class Moneda
{
    /// <summary>
    /// Cultura de Nicaragua: símbolo C$, coma para miles y punto para decimales.
    /// </summary>
    public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-NI");

    /// <summary>
    /// Convierte un monto en texto con el símbolo de córdobas, por ejemplo <c>C$1,250.00</c>.
    /// </summary>
    /// <param name="monto">Monto a mostrar.</param>
    /// <param name="decimales">Cantidad de decimales; 0 para montos resumidos como los del dashboard.</param>
    /// <returns>El monto con formato de moneda.</returns>
    public static string Formatear(decimal monto, int decimales = 2) => monto.ToString("C" + decimales, Cultura);
}
