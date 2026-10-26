using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de los usuarios del sistema.
/// </summary>
public interface IUsuarioDatos
{
    /// <summary>
    /// Busca un usuario por su nombre exacto.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario.</param>
    /// <returns>El usuario encontrado, o <see langword="null"/> si no existe.</returns>
    Usuario? ObtenerPorNombre(string nombreUsuario);

    /// <summary>
    /// Indica si ya existe un usuario con ese nombre.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario.</param>
    /// <returns><see langword="true"/> si el nombre ya está en uso.</returns>
    bool Existe(string nombreUsuario);

    /// <summary>
    /// Registra un usuario nuevo como activo.
    /// </summary>
    /// <param name="usuario">Datos del usuario, con la contraseña ya convertida en hash.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Crear(Usuario usuario);

    /// <summary>
    /// Reemplaza la contraseña de un usuario.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de usuario.</param>
    /// <param name="claveHash">Hash SHA-256 de la nueva contraseña.</param>
    void CambiarClave(string nombreUsuario, string claveHash);

    /// <summary>
    /// Obtiene los usuarios ordenados por nombre.
    /// </summary>
    /// <param name="incluirDesactivados"><see langword="true"/> para incluir los usuarios desactivados.</param>
    /// <returns>Los usuarios encontrados.</returns>
    List<Usuario> Listar(bool incluirDesactivados = false);

    /// <summary>
    /// Desactiva un usuario para que ya no pueda iniciar sesión.
    /// </summary>
    /// <param name="idUsuario">Identificador del usuario.</param>
    void Desactivar(int idUsuario);
}
