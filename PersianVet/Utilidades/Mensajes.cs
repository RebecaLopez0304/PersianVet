namespace PersianVet.Utilidades;

/// <summary>
/// Muestra mensajes comunes de la aplicación.
/// </summary>
public static class Mensajes
{
    public static void MostrarError(Exception excepcion)
    {
        string mensaje = excepcion.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase)
            || excepcion.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
            ? "Ya existe un registro con esa información. Revise los datos e intente nuevamente."
            : "No fue posible completar la operación. Revise la conexión con la base de datos.";

        MessageBox.Show(mensaje, "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

