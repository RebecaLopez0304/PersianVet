namespace PersianVet.Modelos;

/// <summary>
/// Coincidencia encontrada por la búsqueda global.
/// </summary>
public class ResultadoBusqueda
{
    public int Id { get; set; }

    /// <summary>
    /// Cliente, Mascota o Cita.
    /// </summary>
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
}
