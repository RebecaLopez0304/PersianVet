using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Precio en córdobas de un servicio, una vacuna o un medicamento (tabla Tarifas).
/// </summary>
/// <remarks>
/// Cambiar el precio no modifica lo ya registrado: cada cita, consulta, tratamiento y vacuna guarda el
/// precio que tenía al registrarse. Cada cambio queda en <see cref="Historial"/>.
/// </remarks>
public class Tarifa
{
    /// <summary>Tipo de tarifa para los servicios de citas y consultas.</summary>
    public const string Servicio = "Servicio";

    /// <summary>Tipo de tarifa para las vacunas.</summary>
    public const string Vacuna = "Vacuna";

    /// <summary>Tipo de tarifa para los medicamentos de los tratamientos.</summary>
    public const string Medicamento = "Medicamento";

    public int IdTarifa { get; set; }
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la categoría: <see cref="Servicio"/>, <see cref="Vacuna"/> o <see cref="Medicamento"/>.
    /// </summary>
    [DisplayName("Categoría")]
    public string Tipo { get; set; } = Servicio;

    [DisplayName("Precio actual")]
    public decimal Precio { get; set; }

    /// <summary>
    /// Obtiene la fecha desde la que rige el precio actual (último cambio). Requiere cargar <see cref="Historial"/>.
    /// </summary>
    [DisplayName("Vigente desde")]
    public DateTime? VigenteDesde => Historial.Count == 0 ? null : Historial.Max(h => h.Fecha);

    public bool Activa { get; set; } = true;

    [Browsable(false)]
    public List<HistorialPrecio> Historial { get; set; } = [];
}
