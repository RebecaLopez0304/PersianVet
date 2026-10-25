namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Mascotas.
/// </summary>
public class MascotaEntidad
{
    public int IdMascota { get; set; }
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Color { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public string? Observaciones { get; set; }
    public bool Activa { get; set; } = true;

    public ClienteEntidad Cliente { get; set; } = null!;
    public List<CitaEntidad> Citas { get; set; } = [];
    public List<ConsultaEntidad> Consultas { get; set; } = [];
    public List<VacunaEntidad> Vacunas { get; set; } = [];
}
