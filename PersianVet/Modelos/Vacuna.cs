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
    [DisplayName("ID")]
    public int IdVacuna { get; set; }
    public int IdMascota { get; set; }

    [DisplayName("Mascota")]
    public string NombreMascota => Mascota == null ? string.Empty : Mascota.Nombre;

    [DisplayName("Vacuna")]
    public string NombreVacuna { get; set; } = string.Empty;
    [DisplayName("Fecha de aplicación")]
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;

    /// <summary>
    /// Obtiene o establece la fecha del siguiente refuerzo, o <see langword="null"/> si no requiere otra dosis.
    /// </summary>
    [DisplayName("Próxima dosis")]
    public DateTime? ProximaDosis { get; set; }

    public string Lote { get; set; } = string.Empty;
    public decimal Precio { get; set; }

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;
}
