namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información de un veterinario.
/// </summary>
public class Veterinario
{
    public int IdVeterinario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}

