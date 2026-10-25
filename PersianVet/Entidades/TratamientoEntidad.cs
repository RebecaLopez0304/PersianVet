namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Tratamientos.
/// </summary>
public class TratamientoEntidad
{
    public int IdTratamiento { get; set; }
    public int IdConsulta { get; set; }
    public string Medicamento { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
    public string Duracion { get; set; } = string.Empty;
    public string? Indicaciones { get; set; }

    public ConsultaEntidad Consulta { get; set; } = null!;
}
