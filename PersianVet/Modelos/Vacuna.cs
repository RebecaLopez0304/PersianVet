namespace PersianVet.Modelos;

/// <summary>
/// Guarda una vacuna aplicada a una mascota.
/// </summary>
public class Vacuna
{
    public int IdVacuna { get; set; }
    public int IdMascota { get; set; }
    public string Mascota { get; set; } = string.Empty;
    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;
    public DateTime? ProximaDosis { get; set; }
    public string Lote { get; set; } = string.Empty;
}

