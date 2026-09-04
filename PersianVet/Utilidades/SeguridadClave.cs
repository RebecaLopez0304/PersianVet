using System.Security.Cryptography;
using System.Text;

namespace PersianVet.Utilidades;

/// <summary>
/// Convierte una contraseña en un valor que no muestra el texto original.
/// </summary>
public static class SeguridadClave
{
    /// <summary>
    /// Genera el valor SHA-256 de una contraseña.
    /// </summary>
    /// <param name="clave">Contraseña escrita por el usuario.</param>
    /// <returns>Devuelve la contraseña convertida.</returns>
    public static string GenerarHash(string clave)
    {
        byte[] datos = Encoding.UTF8.GetBytes(clave);
        byte[] hash = SHA256.HashData(datos);
        return Convert.ToHexString(hash);
    }
}

