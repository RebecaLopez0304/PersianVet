using System.Net.Mail;

namespace PersianVet.Utilidades;

/// <summary>
/// Contiene validaciones usadas en diferentes formularios.
/// </summary>
public static class Validaciones
{
    /// <summary>
    /// Indica si el texto tiene formato de correo electrónico válido.
    /// </summary>
    /// <param name="correo">Correo a revisar.</param>
    /// <returns><see langword="true"/> si <see cref="System.Net.Mail.MailAddress"/> lo acepta sin cambios.</returns>
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

    /// <summary>
    /// Indica si el teléfono tiene al menos 8 dígitos; se ignoran guiones y espacios.
    /// </summary>
    /// <param name="telefono">Teléfono a revisar.</param>
    /// <returns><see langword="true"/> si tiene 8 dígitos o más.</returns>
    public static bool EsTelefonoValido(string telefono)
    {
        string numeros = new(telefono.Where(char.IsDigit).ToArray());
        return numeros.Length >= 8;
    }
}

