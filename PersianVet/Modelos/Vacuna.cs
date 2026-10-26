using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Vacuna aplicada a una mascota (tabla Vacunas).
/// </summary>
/// <remarks>
/// <see cref="NombreMascota"/> requiere cargar <see cref="Mascota"/> con <c>Include</c>. La relación lleva
/// <c>Browsable(false)</c> para que no aparezca como columna en las tablas.
/// </remarks>
public class Vacuna
{
    public int IdVacuna { get; set; }
    public int IdMascota { get; set; }

    [DisplayName("Mascota")]
    public string NombreMascota => Mascota == null ? string.Empty : Mascota.Nombre;

    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;

    /// <summary>
    /// Obtiene o establece la fecha del siguiente refuerzo, o <see langword="null"/> si no requiere otra dosis.
    /// </summary>
    public DateTime? ProximaDosis { get; set; }

    public string Lote { get; set; } = string.Empty;

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;
}
