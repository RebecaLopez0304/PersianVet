namespace PersianVet.Modelos;

/// <summary>
/// Fila del reporte de actividad clínica por veterinario.
/// </summary>
public class ReporteVeterinario
{
    public string Veterinario { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public int Citas { get; set; }
    public int Completadas { get; set; }
    public int Canceladas { get; set; }
    public int Consultas { get; set; }
    public int Tratamientos { get; set; }

    /// <summary>
    /// Obtiene o establece lo cobrado en C$ por las consultas del veterinario y sus tratamientos.
    /// </summary>
    public decimal Ingresos { get; set; }
}
