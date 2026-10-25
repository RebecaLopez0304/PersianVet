namespace PersianVet.Utilidades;

/// <summary>
/// Muestra mensajes comunes de la aplicación.
/// </summary>
public static class Mensajes
{
    public static void MostrarError(Exception excepcion)
    {
        // Entity Framework envuelve el error de SQL Server en una excepción interna.
        string detalle = excepcion.Message + " " + excepcion.InnerException?.Message;
        string mensaje = detalle.Contains("duplicate", StringComparison.OrdinalIgnoreCase)
            || detalle.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
            ? "Ya existe un registro con esa información. Revise los datos e intente nuevamente."
            : "No fue posible completar la operación. Revise la conexión con la base de datos.";

        MessageBox.Show(mensaje, "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

