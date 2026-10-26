using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Consulta y guarda usuarios mediante Entity Framework Core.
/// </summary>
public class UsuarioDatos : IUsuarioDatos
{
    private readonly ConexionBD conexionBD;

    public UsuarioDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public Usuario? ObtenerPorNombre(string nombreUsuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.AsNoTracking().FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
    }

    /// <inheritdoc/>
    public bool Existe(string nombreUsuario)
    {
        string nombre = nombreUsuario.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.Any(u => u.NombreUsuario == nombre);
    }

    /// <inheritdoc/>
    public int Crear(Usuario usuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        usuario.NombreUsuario = usuario.NombreUsuario.Trim();
        usuario.Activo = true;
        contexto.Usuarios.Add(usuario);
        contexto.SaveChanges();
        return usuario.IdUsuario;
    }

    /// <inheritdoc/>
    public void CambiarClave(string nombreUsuario, string claveHash)
    {
        string nombre = nombreUsuario.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Usuarios.Where(u => u.NombreUsuario == nombre)
            .ExecuteUpdate(u => u.SetProperty(x => x.ClaveHash, claveHash));
    }

    /// <inheritdoc/>
    public List<Usuario> Listar(bool incluirDesactivados = false)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.AsNoTracking()
            .Where(u => incluirDesactivados || u.Activo)
            .OrderBy(u => u.NombreUsuario)
            .ToList();
    }

    /// <inheritdoc/>
    public void Desactivar(int idUsuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Usuarios.Where(u => u.IdUsuario == idUsuario)
            .ExecuteUpdate(u => u.SetProperty(x => x.Activo, false));
    }
}
