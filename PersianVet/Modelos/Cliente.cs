using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Guarda la información del propietario de una mascota (tabla Clientes).
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

    // Propiedad calculada: no se guarda en la base de datos.
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    // Relación de EF Core; Browsable(false) evita que aparezca como columna en las tablas.
    [Browsable(false)]
    public List<Mascota> Mascotas { get; set; } = [];
}
