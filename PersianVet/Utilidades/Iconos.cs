using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PersianVet.Utilidades;

/// <summary>
/// Carga los iconos del menú desde Recursos\Iconos (Material Icons de Google, licencia Apache 2.0).
/// </summary>
public static class Iconos
{
    /// <summary>
    /// Carga un icono, lo ajusta al tamaño pedido y lo pinta del color indicado.
    /// </summary>
    /// <param name="archivo">Nombre del archivo sin extensión, por ejemplo "inicio".</param>
    /// <param name="color">Color con el que se muestra el icono.</param>
    /// <param name="tamano">Tamaño final en píxeles.</param>
    /// <returns>El icono listo para usar, o null si el archivo no existe.</returns>
    public static Image? Cargar(string archivo, Color color, int tamano)
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, "Recursos", "Iconos", archivo + ".png");
        if (!File.Exists(ruta))
        {
            return null;
        }

        using Image original = Image.FromFile(ruta);
        Bitmap icono = new(tamano, tamano);
        using Graphics dibujo = Graphics.FromImage(icono);
        dibujo.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Los iconos vienen en negro: la matriz cambia ese negro por el color pedido y conserva la transparencia.
        ColorMatrix cambioDeColor = new(
        [
            [0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0],
            [0, 0, 0, 1, 0],
            [color.R / 255f, color.G / 255f, color.B / 255f, 0, 1]
        ]);
        using ImageAttributes atributos = new();
        atributos.SetColorMatrix(cambioDeColor);

        dibujo.DrawImage(
            original,
            new Rectangle(0, 0, tamano, tamano),
            0, 0, original.Width, original.Height,
            GraphicsUnit.Pixel,
            atributos);
        return icono;
    }
}
