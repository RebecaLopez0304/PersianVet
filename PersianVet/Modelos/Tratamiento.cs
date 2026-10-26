namespace PersianVet.Modelos;

/// <summary>
/// Guarda un medicamento indicado durante una consulta.
/// </summary>
public class Tratamiento
{
    public int IdTratamiento { get; set; }
    public int IdConsulta { get; set; }
    public string Medicamento { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
    public string Duracion { get; set; } = string.Empty;
    public string Indicaciones { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}

