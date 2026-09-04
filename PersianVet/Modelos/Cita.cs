namespace PersianVet.Modelos;

/// <summary>
/// Guarda los datos de una cita veterinaria.
/// </summary>
public class Cita
{
    public int IdCita { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Programada";
    public string Mascota { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string Veterinario { get; set; } = string.Empty;
}

