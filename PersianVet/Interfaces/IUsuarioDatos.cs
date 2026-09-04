using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las consultas necesarias para iniciar sesión.
/// </summary>
public interface IUsuarioDatos
{
    Usuario? ObtenerPorNombre(string nombreUsuario);
    bool Existe(string nombreUsuario);
    int Crear(Usuario usuario);
    void CambiarClave(string nombreUsuario, string claveHash);
    List<Usuario> Listar(bool incluirDesactivados = false);
    void Desactivar(int idUsuario);
}
