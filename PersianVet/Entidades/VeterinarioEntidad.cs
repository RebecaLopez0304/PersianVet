namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Veterinarios.
/// </summary>
public class VeterinarioEntidad
{
    public int IdVeterinario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public List<CitaEntidad> Citas { get; set; } = [];
    public List<ConsultaEntidad> Consultas { get; set; } = [];
}
