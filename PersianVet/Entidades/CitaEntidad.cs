namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Citas.
/// </summary>
public class CitaEntidad
{
    public int IdCita { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Programada";

    public MascotaEntidad Mascota { get; set; } = null!;
    public VeterinarioEntidad Veterinario { get; set; } = null!;
}
