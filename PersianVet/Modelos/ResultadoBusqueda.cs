using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Coincidencia encontrada por la búsqueda global. Cada dato va en su propia columna;
/// las que no aplican a un tipo de resultado quedan vacías.
/// </summary>
public class ResultadoBusqueda
{
    public int Id { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de resultado: Cliente, Mascota o Cita.
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre del cliente o de la mascota (en las citas, la mascota citada).
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el correo del cliente, la raza de la mascota o el motivo de la cita.
    /// </summary>
    [DisplayName("Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [DisplayName("Dueño")]
    public string Propietario { get; set; } = string.Empty;

    [DisplayName("Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [DisplayName("Fecha y hora")]
    public DateTime? Fecha { get; set; }

    public string Estado { get; set; } = string.Empty;
}
