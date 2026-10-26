using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Veterinario que atiende citas y consultas (tabla Veterinarios).
/// </summary>
/// <remarks>
/// Las propiedades de relación llevan <c>Browsable(false)</c> para que no aparezcan como columnas en las tablas.
/// </remarks>
public class Veterinario
{
    [DisplayName("ID")]
    public int IdVeterinario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    [DisplayName("Teléfono")]
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Obtiene los nombres y apellidos juntos. Es calculada y no se guarda en la base de datos.
    /// </summary>
    [DisplayName("Nombre completo")]
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    [Browsable(false)]
    public List<Cita> Citas { get; set; } = [];

    [Browsable(false)]
    public List<Consulta> Consultas { get; set; } = [];
}
