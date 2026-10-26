using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda los datos de una cita veterinaria (tabla Citas).
/// </summary>
public class Cita
{
    public int IdCita { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Programada";

    // Propiedades calculadas a partir de los datos cargados con Include.
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
