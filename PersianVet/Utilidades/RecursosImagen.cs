namespace PersianVet.Utilidades;

/// <summary>
/// Carga las imágenes incluidas en la carpeta Recursos.
/// </summary>
public static class RecursosImagen
{
    /// <summary>
    /// Carga la imagen de la mascota que se muestra en el menú.
    /// </summary>
    /// <returns>La imagen, o <see langword="null"/> si el archivo no existe.</returns>
    public static Image? CargarMascota()
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, "Recursos", "mascota-persian.png");
        return File.Exists(ruta) ? Image.FromFile(ruta) : null;
    }
}

