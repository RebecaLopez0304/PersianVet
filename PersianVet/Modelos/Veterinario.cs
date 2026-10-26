using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información de un veterinario (tabla Veterinarios).
/// </summary>
public class Veterinario
{
    public int IdVeterinario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    // Propiedad calculada: no se guarda en la base de datos.
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    [Browsable(false)]
    public List<Cita> Citas { get; set; } = [];

    [Browsable(false)]
    public List<Consulta> Consultas { get; set; } = [];
}
