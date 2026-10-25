namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Consultas.
/// </summary>
public class ConsultaEntidad
{
    public int IdConsulta { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    public MascotaEntidad Mascota { get; set; } = null!;
    public VeterinarioEntidad Veterinario { get; set; } = null!;
    public List<TratamientoEntidad> Tratamientos { get; set; } = [];
}
