using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda una vacuna aplicada a una mascota (tabla Vacunas).
/// </summary>
public class Vacuna
{
    public int IdVacuna { get; set; }
    public int IdMascota { get; set; }

    // Propiedad calculada a partir de la mascota cargada con Include.
    [DisplayName("Mascota")]
    public string NombreMascota => Mascota == null ? string.Empty : Mascota.Nombre;

    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;
    public DateTime? ProximaDosis { get; set; }
    public string Lote { get; set; } = string.Empty;

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;
}
