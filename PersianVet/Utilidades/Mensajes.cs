using PersianVet.Modelos;

namespace PersianVet.Utilidades;

/// <summary>
/// Muestra mensajes comunes de la aplicación.
/// </summary>
public static class Mensajes
{
    /// <summary>
    /// Registra el error en errores.log y muestra al usuario un mensaje comprensible en lugar del técnico.
    /// </summary>
    /// <param name="excepcion">Error ocurrido.</param>
    public static void MostrarError(Exception excepcion)
    {
        RegistrarError(excepcion);

        // Entity Framework envuelve el error de SQL Server en una excepción interna.
        string detalle = excepcion.Message + " " + excepcion.InnerException?.Message;
        string mensaje = detalle.Contains("duplicate", StringComparison.OrdinalIgnoreCase)
            || detalle.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
            ? "Ya existe un registro con esa información. Revise los datos e intente nuevamente."
            : "No fue posible completar la operación. Revise la conexión con la base de datos.";

        MessageBox.Show(mensaje, "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Ejecuta una operación de un servicio, muestra su mensaje y, si salió bien, ejecuta la acción indicada.
    /// </summary>
    /// <param name="operacion">Llamada al servicio, por ejemplo <c>() => clienteServicio.Guardar(cliente)</c>.</param>
    /// <param name="siSaleBien">Acción posterior, por ejemplo limpiar el formulario y recargar la tabla.</param>
    public static void EjecutarOperacion(Func<ResultadoOperacion> operacion, Action siSaleBien)
    {
        try
        {
            ResultadoOperacion resultado = operacion();
            MessageBox.Show(
                resultado.Mensaje,
                "PersianVet",
                MessageBoxButtons.OK,
                resultado.Exitoso ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exitoso)
            {
                siSaleBien();
            }
        }
        catch (Exception excepcion)
        {
            MostrarError(excepcion);
        }
    }

    /// <summary>
    /// Guarda el error real en errores.log (junto al ejecutable) para poder diagnosticarlo.
    /// </summary>
    private static void RegistrarError(Exception excepcion)
    {
        try
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "errores.log");
            File.AppendAllText(ruta, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {excepcion}{Environment.NewLine}{Environment.NewLine}");
        }
        catch
        {
            // Registrar el error nunca debe provocar otro error.
        }
    }
}
