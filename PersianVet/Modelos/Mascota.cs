using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Mascota atendida en la veterinaria (tabla Mascotas).
/// </summary>
/// <remarks>
/// Las propiedades de relación llevan <c>Browsable(false)</c> para que no aparezcan como columnas en las tablas.
/// </remarks>
public class Mascota
{
    [DisplayName("ID")]
    public int IdMascota { get; set; }
    public int IdCliente { get; set; }

    /// <summary>
    /// Obtiene el nombre del dueño. Requiere cargar <see cref="Cliente"/> con <c>Include</c>.
    /// </summary>
    public string Propietario => Cliente == null ? string.Empty : Cliente.NombreCompleto;

    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    [DisplayName("Fecha de nacimiento")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;
    public string Color { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;

    [Browsable(false)]
    public Cliente Cliente { get; set; } = null!;

    [Browsable(false)]
    public List<Cita> Citas { get; set; } = [];

    [Browsable(false)]
    public List<Consulta> Consultas { get; set; } = [];

    [Browsable(false)]
    public List<Vacuna> Vacunas { get; set; } = [];
}
