namespace PersianVet.Modelos;

/// <summary>
/// Guarda los datos usados para ingresar al sistema.
/// </summary>
public class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string ClaveHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

