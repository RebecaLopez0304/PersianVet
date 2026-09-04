using System.Net.Mail;

namespace PersianVet.Utilidades;

/// <summary>
/// Contiene validaciones usadas en diferentes formularios.
/// </summary>
public static class Validaciones
{
    public static bool EsCorreoValido(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            return false;
        }

        try
        {
            MailAddress direccion = new(correo);
            return direccion.Address == correo.Trim();
        }
        catch
        {
            return false;
        }
    }

    public static bool EsTelefonoValido(string telefono)
    {
        string numeros = new(telefono.Where(char.IsDigit).ToArray());
        return numeros.Length >= 8;
    }
}

