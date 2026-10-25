namespace PersianVet.Modelos;

/// <summary>
/// Fila del reporte de atención agrupada por especie.
/// </summary>
public class ReporteEspecie
{
    public string Especie { get; set; } = string.Empty;
    public int Mascotas { get; set; }
    public int Consultas { get; set; }
    public int Vacunas { get; set; }
}
