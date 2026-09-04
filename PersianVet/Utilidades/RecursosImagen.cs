namespace PersianVet.Utilidades;

/// <summary>
/// Carga las imágenes incluidas en la carpeta Recursos.
/// </summary>
public static class RecursosImagen
{
    public static Image? CargarMascota()
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, "Recursos", "mascota-persian.png");
        return File.Exists(ruta) ? Image.FromFile(ruta) : null;
    }
}

