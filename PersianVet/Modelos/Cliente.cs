using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Propietario de una o varias mascotas (tabla Clientes).
/// </summary>
/// <remarks>
/// Las propiedades de relación llevan <c>Browsable(false)</c> para que no aparezcan como columnas en las tablas.
/// </remarks>
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

    /// <summary>
    /// Obtiene los nombres y apellidos juntos. Es calculada y no se guarda en la base de datos.
    /// </summary>
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    [Browsable(false)]
    public List<Mascota> Mascotas { get; set; } = [];
}
