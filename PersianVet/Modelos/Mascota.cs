namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información de una mascota atendida en la veterinaria.
/// </summary>
public class Mascota
{
    public int IdMascota { get; set; }
    public int IdCliente { get; set; }
    public string Propietario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raza { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;
    public string Color { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public string Observaciones { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
}

