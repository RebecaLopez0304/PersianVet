using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Revisa los datos usados para iniciar sesión.
/// </summary>
public class UsuarioServicio
{
    private readonly IUsuarioDatos usuarioDatos;

    public UsuarioServicio(IUsuarioDatos usuarioDatos)
    {
        this.usuarioDatos = usuarioDatos;
    }

    /// <summary>
    /// Comprueba que el usuario exista, esté activo y que la contraseña coincida.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario.</param>
    /// <param name="clave">Contraseña escrita por el usuario.</param>
    /// <returns>El resultado del inicio de sesión y, si es correcto, el usuario.</returns>
    public ResultadoInicioSesion IniciarSesion(string nombreUsuario, string clave)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(clave))
        {
            return new ResultadoInicioSesion { Mensaje = "Escriba el usuario y la contraseña." };
        }

        Usuario? usuario = usuarioDatos.ObtenerPorNombre(nombreUsuario.Trim());
        if (usuario is null || usuario.ClaveHash != SeguridadClave.GenerarHash(clave))
        {
            return new ResultadoInicioSesion { Mensaje = "El usuario o la contraseña no son correctos." };
        }

        if (!usuario.Activo)
        {
            return new ResultadoInicioSesion { Mensaje = "El usuario se encuentra inactivo." };
        }

        return new ResultadoInicioSesion
        {
            Exitoso = true,
            Mensaje = "Inicio de sesión correcto.",
            Usuario = usuario
        };
    }

    /// <summary>
    /// Valida los datos y crea un usuario nuevo; la contraseña se guarda como hash SHA-256.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario; no puede estar en uso.</param>
    /// <param name="clave">Contraseña de al menos 4 caracteres.</param>
    /// <param name="confirmacion">Contraseña repetida; debe coincidir con <paramref name="clave"/>.</param>
    /// <param name="rol">Rol del usuario; si se deja vacío se usa Recepcionista.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion CrearUsuario(string nombreUsuario, string clave, string confirmacion, string rol)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(clave))
        {
            return ResultadoOperacion.Error("Escriba el usuario y la contraseña.");
        }

        if (clave != confirmacion)
        {
            return ResultadoOperacion.Error("Las contraseñas no coinciden.");
        }

        if (clave.Length < 4)
        {
            return ResultadoOperacion.Error("La contraseña debe tener al menos 4 caracteres.");
        }

        if (usuarioDatos.Existe(nombreUsuario))
        {
            return ResultadoOperacion.Error("El nombre de usuario ya está registrado.");
        }

        Usuario usuario = new()
        {
            NombreUsuario = nombreUsuario.Trim(),
            ClaveHash = SeguridadClave.GenerarHash(clave),
            Rol = string.IsNullOrWhiteSpace(rol) ? "Recepcionista" : rol,
            Activo = true
        };

        usuario.IdUsuario = usuarioDatos.Crear(usuario);
        return ResultadoOperacion.Correcto("El usuario se creó correctamente.");
    }

    /// <summary>
    /// Valida la nueva contraseña y la guarda como hash SHA-256.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de un usuario registrado.</param>
    /// <param name="nuevaClave">Contraseña nueva de al menos 4 caracteres.</param>
    /// <param name="confirmacion">Contraseña repetida; debe coincidir con <paramref name="nuevaClave"/>.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion CambiarClave(string nombreUsuario, string nuevaClave, string confirmacion)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(nuevaClave))
        {
            return ResultadoOperacion.Error("Escriba el usuario y la nueva contraseña.");
        }

        if (!usuarioDatos.Existe(nombreUsuario))
        {
            return ResultadoOperacion.Error("El usuario no está registrado.");
        }

        if (nuevaClave != confirmacion)
        {
            return ResultadoOperacion.Error("Las contraseñas no coinciden.");
        }

        if (nuevaClave.Length < 4)
        {
            return ResultadoOperacion.Error("La contraseña debe tener al menos 4 caracteres.");
        }

        usuarioDatos.CambiarClave(nombreUsuario.Trim(), SeguridadClave.GenerarHash(nuevaClave));
        return ResultadoOperacion.Correcto("La contraseña se cambió correctamente.");
    }

    /// <inheritdoc cref="IUsuarioDatos.Listar(bool)"/>
    public List<Usuario> Listar(bool incluirDesactivados = false)
    {
        return usuarioDatos.Listar(incluirDesactivados);
    }

    /// <summary>
    /// Desactiva un usuario; el usuario admin no se puede desactivar.
    /// </summary>
    /// <param name="usuario">Usuario seleccionado.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Desactivar(Usuario usuario)
    {
        if (usuario.IdUsuario <= 0)
        {
            return ResultadoOperacion.Error("Seleccione un usuario de la lista.");
        }

        if (usuario.NombreUsuario.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            return ResultadoOperacion.Error("El usuario admin no se puede desactivar.");
        }

        usuarioDatos.Desactivar(usuario.IdUsuario);
        return ResultadoOperacion.Correcto("El usuario se desactivó correctamente.");
    }
}
