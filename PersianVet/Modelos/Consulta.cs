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
    [DisplayName("Fecha y hora")]
    public DateTime FechaHora { get; set; } = DateTime.Now;

    /// <summary>
    /// Obtiene o establece el nombre de la tarifa del servicio a realizar.
    /// </summary>
    public string Servicio { get; set; } = string.Empty;

    public string Motivo { get; set; } = string.Empty;
    [DisplayName("Diagnóstico")]
    public string Diagnostico { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el precio en C$ del servicio, tomado de su tarifa al registrar la consulta.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Obtiene el total en C$: precio del servicio más el de cada tratamiento.
    /// </summary>
    public decimal Total => Precio + ListaTratamientos.Sum(t => t.Precio);

    [DisplayName("Veterinario")]
    public string NombreVeterinario => Veterinario == null ? string.Empty : Veterinario.NombreCompleto;

    /// <summary>
    /// Obtiene los tratamientos en una sola línea, por ejemplo "Meloxicam - 1.5 mg; Glucosamina - 500 mg".
    /// </summary>
    public string Tratamientos => string.Join(", ", ListaTratamientos.Select(t => $"{t.Medicamento} {t.Dosis}"));

    [Browsable(false)]
    public List<Tratamiento> ListaTratamientos { get; set; } = [];

    [Browsable(false)]
    public Mascota Mascota { get; set; } = null!;

    [Browsable(false)]
    public Veterinario Veterinario { get; set; } = null!;
}
