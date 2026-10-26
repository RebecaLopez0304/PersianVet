using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Datos médicos registrados durante una consulta (tabla Consultas).
/// </summary>
/// <remarks>
/// <see cref="NombreVeterinario"/> y <see cref="Tratamientos"/> requieren cargar <see cref="Veterinario"/> y
/// <see cref="ListaTratamientos"/> con <c>Include</c>. Las relaciones llevan <c>Browsable(false)</c>
/// para que no aparezcan como columnas en las tablas.
/// </remarks>
public class Consulta
{
    public int IdConsulta { get; set; }
    public int IdMascota { get; set; }
    public int IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;

    [DisplayName("Veterinario")]
    public string NombreVeterinario => Veterinario == null ? string.Empty : Veterinario.NombreCompleto;

    /// <summary>
    /// Obtiene los tratamientos en una sola línea, por ejemplo "Meloxicam - 1.5 mg; Glucosamina - 500 mg".
    /// </summary>
    public string Tratamientos => string.Join("; ", ListaTratamientos.Select(t => $"{t.Medicamento} - {t.Dosis}"));

    [Browsable(false)]
    public List<Tratamiento> ListaTratamientos { get; set; } = [];

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;

    [Browsable(false)]
    public Veterinario Veterinario { get; set; } = null!;
}
