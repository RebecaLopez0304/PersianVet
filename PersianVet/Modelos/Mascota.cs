using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información de una mascota atendida en la veterinaria (tabla Mascotas).
/// </summary>
public class Mascota
{
    public int IdMascota { get; set; }
    public int IdCliente { get; set; }

    // Propiedad calculada a partir del cliente cargado con Include.
    public string Propietario => Cliente == null ? string.Empty : Cliente.NombreCompleto;

    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
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
