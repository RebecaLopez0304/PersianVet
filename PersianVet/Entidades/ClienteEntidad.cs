namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Clientes.
/// </summary>
public class ClienteEntidad
{
    public int IdCliente { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; } = true;

    public List<MascotaEntidad> Mascotas { get; set; } = [];
}
