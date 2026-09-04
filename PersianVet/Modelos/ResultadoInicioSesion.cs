namespace PersianVet.Modelos;

/// <summary>
/// Contiene el resultado del inicio de sesión y el usuario encontrado.
/// </summary>
public class ResultadoInicioSesion
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public Usuario? Usuario { get; set; }
}

