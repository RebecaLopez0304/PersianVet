namespace PersianVet.Modelos;

/// <summary>
/// Guarda los datos médicos registrados durante una consulta.
/// </summary>
public class Consulta
{
    public int IdConsulta { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public string Veterinario { get; set; } = string.Empty;
    public string Tratamientos { get; set; } = string.Empty;
    public List<Tratamiento> ListaTratamientos { get; set; } = [];
}

