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
    /// Crea un usuario nuevo para ingresar al sistema.
    /// </summary>
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
    /// Cambia la contraseña de un usuario registrado.
    /// </summary>
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

    public List<Usuario> Listar(bool incluirDesactivados = false)
    {
        return usuarioDatos.Listar(incluirDesactivados);
    }

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
