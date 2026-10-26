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

        MessageBox.Show(TraducirError(excepcion), "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Ejecuta una operación de un servicio y muestra su mensaje. Si sale bien ejecuta la acción indicada;
    /// si no, no toca el formulario, así el usuario conserva lo que escribió y puede corregirlo.
    /// </summary>
    /// <param name="operacion">Llamada al servicio, por ejemplo <c>() => clienteServicio.Guardar(cliente)</c>.</param>
    /// <param name="siSaleBien">Acción posterior, por ejemplo limpiar el formulario y recargar la tabla.</param>
    /// <returns><see langword="true"/> si la operación se completó.</returns>
    public static bool EjecutarOperacion(Func<ResultadoOperacion> operacion, Action? siSaleBien = null)
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
                siSaleBien?.Invoke();
            }

            return resultado.Exitoso;
        }
        catch (Exception excepcion)
        {
            MostrarError(excepcion);
            return false;
        }
    }

    /// <summary>
    /// Convierte los errores más comunes de SQL Server en un mensaje que el usuario entiende.
    /// </summary>
    private static string TraducirError(Exception excepcion)
    {
        // Entity Framework envuelve el error de SQL Server en una excepción interna.
        string detalle = excepcion.Message + " " + excepcion.InnerException?.Message;

        if (detalle.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
        {
            return "Ya existe un registro con ese dato, que no se puede repetir. Revise el correo, " +
                "el nombre de usuario o el nombre de la tarifa e intente nuevamente.";
        }

        if (detalle.Contains("truncated", StringComparison.OrdinalIgnoreCase))
        {
            return "Uno de los textos es más largo de lo permitido. Acórtelo e intente nuevamente.";
        }

        if (detalle.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
        {
            return "El registro relacionado (cliente, mascota o veterinario) ya no existe. Actualice la lista e intente nuevamente.";
        }

        if (detalle.Contains("CHECK constraint", StringComparison.OrdinalIgnoreCase))
        {
            return "Un dato no cumple las reglas de la base de datos (por ejemplo, un precio o peso en cero). Revise los valores.";
        }

        return "No fue posible completar la operación. Revise la conexión con la base de datos.";
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
