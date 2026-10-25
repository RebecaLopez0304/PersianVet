namespace PersianVet.Entidades;

/// <summary>
/// Entidad de EF Core asociada a la tabla Usuarios.
/// </summary>
public class UsuarioEntidad
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string ClaveHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
