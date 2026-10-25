using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Gráfico de barras sencillo dibujado con GDI+: una barra por cada punto.
/// </summary>
public class GraficoBarras : Control
{
    private List<PuntoGrafico> puntos = [];

    public string Titulo { get; set; } = string.Empty;

    public GraficoBarras()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F);
    }

    /// <summary>
    /// Reemplaza los datos y vuelve a dibujar el gráfico.
    /// </summary>
    public void AsignarDatos(IEnumerable<PuntoGrafico> datos)
    {
        puntos = datos.ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using Font fuenteTitulo = new("Segoe UI", 10F, FontStyle.Bold);
        using SolidBrush pincelTitulo = new(Tema.AzulOscuro);
        using SolidBrush pincelTexto = new(Tema.GrisOscuro);
        using SolidBrush pincelBarra = new(Tema.Naranja);
        g.DrawString(Titulo, fuenteTitulo, pincelTitulo, 10, 8);

        if (puntos.Count == 0)
        {
            g.DrawString("Sin datos para mostrar", Font, pincelTexto, 10, 40);
            return;
        }

        // Zona de dibujo: debajo del título y por encima de las etiquetas.
        int alturaEtiqueta = 20;
        Rectangle zona = new(10, 56, Width - 20, Height - 56 - alturaEtiqueta);
        int maximo = Math.Max(1, puntos.Max(p => p.Valor));
        float anchoColumna = zona.Width / (float)puntos.Count;

        using StringFormat centrado = new() { Alignment = StringAlignment.Center };
        for (int i = 0; i < puntos.Count; i++)
        {
            float alto = zona.Height * puntos[i].Valor / maximo;
            float x = zona.X + i * anchoColumna + anchoColumna * 0.2F;
            float ancho = anchoColumna * 0.6F;
            float y = zona.Bottom - alto;

            g.FillRectangle(pincelBarra, x, y, ancho, alto);
            g.DrawString(puntos[i].Valor.ToString(), Font, pincelTexto, x + ancho / 2, y - 18, centrado);
            g.DrawString(puntos[i].Etiqueta, Font, pincelTexto, x + ancho / 2, zona.Bottom + 3, centrado);
        }

        using Pen eje = new(Tema.Borde);
        g.DrawLine(eje, zona.X, zona.Bottom, zona.Right, zona.Bottom);
    }
}
