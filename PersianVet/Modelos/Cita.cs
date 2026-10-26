using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Cita programada para una mascota con un veterinario (tabla Citas).
/// </summary>
/// <remarks>
/// Las propiedades <c>Nombre*</c> requieren cargar <see cref="Mascota"/> (con su cliente) y
/// <see cref="Veterinario"/> con <c>Include</c>. Las relaciones llevan <c>Browsable(false)</c>
/// para que no aparezcan como columnas en las tablas.
/// </remarks>
public class Cita
{
    public int IdCita { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el estado: Programada, Confirmada, Completada o Cancelada.
    /// </summary>
    public string Estado { get; set; } = "Programada";

    [DisplayName("Mascota")]
    public string NombreMascota => Mascota == null ? string.Empty : Mascota.Nombre;

    [DisplayName("Cliente")]
    public string NombreCliente => Mascota?.Cliente == null ? string.Empty : Mascota.Cliente.NombreCompleto;

    [DisplayName("Veterinario")]
    public string NombreVeterinario => Veterinario == null ? string.Empty : Veterinario.NombreCompleto;

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;

    [Browsable(false)]
    public Veterinario Veterinario { get; set; } = null!;
}
