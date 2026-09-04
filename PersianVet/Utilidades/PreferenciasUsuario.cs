namespace PersianVet.Utilidades;

/// <summary>
/// Guarda el nombre del último usuario cuando se solicita recordarlo.
/// </summary>
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
