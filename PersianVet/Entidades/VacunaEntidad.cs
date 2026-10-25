namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Vacunas.
/// </summary>
public class VacunaEntidad
{
    public int IdVacuna { get; set; }
    public int IdMascota { get; set; }
    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; }
    public DateTime? ProximaDosis { get; set; }
    public string Lote { get; set; } = string.Empty;

    public MascotaEntidad Mascota { get; set; } = null!;
}
