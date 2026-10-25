namespace PersianVet.Utilidades;

/// <summary>
/// Dibuja iconos pequeños para los botones del menú.
/// </summary>
public static class Iconos
{
    /// <summary>
    /// Crea un icono sencillo según el nombre recibido.
    /// </summary>
    /// <param name="nombre">Nombre del icono que se desea dibujar.</param>
    /// <param name="color">Color usado para las líneas.</param>
    /// <returns>Devuelve una imagen de 22 por 22 píxeles.</returns>
    public static Image Crear(string nombre, Color color)
    {
        Bitmap imagen = new(22, 22);
        using Graphics dibujo = Graphics.FromImage(imagen);
        using Pen lapiz = new(color, 2F);
        dibujo.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        switch (nombre)
        {
            case "Inicio":
                dibujo.DrawLines(lapiz, new Point[] { new(3, 10), new(11, 3), new(19, 10) });
                dibujo.DrawRectangle(lapiz, 6, 9, 10, 9);
                break;
            case "Clientes":
                dibujo.DrawEllipse(lapiz, 7, 3, 7, 7);
                dibujo.DrawArc(lapiz, 3, 10, 15, 10, 180, 180);
                break;
            case "Mascotas":
                dibujo.DrawEllipse(lapiz, 7, 9, 8, 8);
                dibujo.DrawEllipse(lapiz, 2, 6, 4, 5);
                dibujo.DrawEllipse(lapiz, 6, 2, 4, 5);
                dibujo.DrawEllipse(lapiz, 12, 2, 4, 5);
                dibujo.DrawEllipse(lapiz, 16, 6, 4, 5);
                break;
            case "Citas":
                dibujo.DrawRectangle(lapiz, 3, 5, 16, 14);
                dibujo.DrawLine(lapiz, 3, 9, 19, 9);
                dibujo.DrawLine(lapiz, 7, 2, 7, 7);
                dibujo.DrawLine(lapiz, 15, 2, 15, 7);
                break;
            case "Consultas":
                dibujo.DrawEllipse(lapiz, 3, 3, 16, 16);
                dibujo.DrawLine(lapiz, 11, 6, 11, 16);
                dibujo.DrawLine(lapiz, 6, 11, 16, 11);
                break;
            case "Vacunas":
                dibujo.DrawLine(lapiz, 5, 17, 17, 5);
                dibujo.DrawLine(lapiz, 8, 18, 4, 14);
                dibujo.DrawLine(lapiz, 18, 8, 14, 4);
                dibujo.DrawLine(lapiz, 16, 3, 19, 6);
                break;
            case "Veterinarios":
                dibujo.DrawEllipse(lapiz, 7, 2, 8, 8);
                dibujo.DrawArc(lapiz, 3, 10, 16, 10, 180, 180);
                dibujo.DrawLine(lapiz, 17, 2, 17, 8);
                dibujo.DrawLine(lapiz, 14, 5, 20, 5);
                break;
            case "Reportes":
                dibujo.DrawRectangle(lapiz, 3, 12, 3, 7);
                dibujo.DrawRectangle(lapiz, 9, 8, 3, 11);
                dibujo.DrawRectangle(lapiz, 15, 3, 3, 16);
                break;
            default:
                dibujo.DrawRectangle(lapiz, 4, 3, 10, 16);
                dibujo.DrawLine(lapiz, 9, 11, 20, 11);
                dibujo.DrawLines(lapiz, new Point[] { new(16, 7), new(20, 11), new(16, 15) });
                break;
        }

        return imagen;
    }
}
