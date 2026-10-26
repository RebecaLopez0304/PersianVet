namespace PersianVet.Utilidades;

/// <summary>
/// Guarda el nombre del último usuario cuando se marca "Recordarme".
/// </summary>
/// <remarks>
/// Se guarda en <c>%LOCALAPPDATA%\PersianVet\usuario.txt</c>, fuera de la carpeta del programa.
/// </remarks>
public static class PreferenciasUsuario
{
    private static readonly string Carpeta = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersianVet");
    private static readonly string Archivo = Path.Combine(Carpeta, "usuario.txt");

    public static void Guardar(string usuario)
    {
        Directory.CreateDirectory(Carpeta);
        File.WriteAllText(Archivo, usuario);
    }

    /// <summary>
    /// Lee el último usuario recordado.
    /// </summary>
    /// <returns>El nombre guardado, o una cadena vacía si no hay ninguno.</returns>
    public static string Leer()
    {
        return File.Exists(Archivo) ? File.ReadAllText(Archivo).Trim() : string.Empty;
    }

    public static void Borrar()
    {
        if (File.Exists(Archivo))
        {
            File.Delete(Archivo);
        }
    }
}
