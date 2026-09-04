namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información del propietario de una mascota.
/// </summary>
public class Cliente
{
    public int IdCliente { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Today;
    public bool Activo { get; set; } = true;
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}

