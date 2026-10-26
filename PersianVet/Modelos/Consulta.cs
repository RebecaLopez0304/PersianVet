using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda los datos médicos registrados durante una consulta (tabla Consultas).
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

    // Propiedades calculadas a partir de los datos cargados con Include.
    [DisplayName("Veterinario")]
    public string NombreVeterinario => Veterinario == null ? string.Empty : Veterinario.NombreCompleto;

    public string Tratamientos => string.Join("; ", ListaTratamientos.Select(t => $"{t.Medicamento} - {t.Dosis}"));

    [Browsable(false)]
    public List<Tratamiento> ListaTratamientos { get; set; } = [];

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;

    [Browsable(false)]
    public Veterinario Veterinario { get; set; } = null!;
}
