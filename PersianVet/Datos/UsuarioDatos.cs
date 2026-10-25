using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
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

    public Usuario? ObtenerPorNombre(string nombreUsuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.AsNoTracking()
            .Where(u => u.NombreUsuario == nombreUsuario)
            .Select(u => new Usuario
            {
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                ClaveHash = u.ClaveHash,
                Rol = u.Rol,
                Activo = u.Activo
            })
            .FirstOrDefault();
    }

    public bool Existe(string nombreUsuario)
    {
        string nombre = nombreUsuario.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.Any(u => u.NombreUsuario == nombre);
    }

    public int Crear(Usuario usuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        UsuarioEntidad entidad = new()
        {
            NombreUsuario = usuario.NombreUsuario.Trim(),
            ClaveHash = usuario.ClaveHash,
            Rol = usuario.Rol,
            Activo = true
        };
        contexto.Usuarios.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdUsuario;
    }

    public void CambiarClave(string nombreUsuario, string claveHash)
    {
        string nombre = nombreUsuario.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Usuarios.Where(u => u.NombreUsuario == nombre)
            .ExecuteUpdate(u => u.SetProperty(x => x.ClaveHash, claveHash));
    }

    public List<Usuario> Listar(bool incluirDesactivados = false)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Usuarios.AsNoTracking()
            .Where(u => incluirDesactivados || u.Activo)
            .OrderBy(u => u.NombreUsuario)
            .Select(u => new Usuario
            {
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                ClaveHash = u.ClaveHash,
                Rol = u.Rol,
                Activo = u.Activo
            })
            .ToList();
    }

    public void Desactivar(int idUsuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Usuarios.Where(u => u.IdUsuario == idUsuario)
            .ExecuteUpdate(u => u.SetProperty(x => x.Activo, false));
    }
}
