using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de veterinarios mediante Entity Framework Core.
/// </summary>
public class VeterinarioDatos : IVeterinarioDatos
{
    private readonly ConexionBD conexionBD;

    public VeterinarioDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Veterinario> Listar(string filtro, bool soloActivos = true)
    {
        string texto = filtro.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();

        return contexto.Veterinarios.AsNoTracking()
            .Where(v => (!soloActivos || v.Activo)
                && (texto == ""
                    || v.Nombres.Contains(texto)
                    || v.Apellidos.Contains(texto)
                    || v.Especialidad.Contains(texto)))
            .OrderByDescending(v => v.IdVeterinario)
            .Select(v => new Veterinario
            {
                IdVeterinario = v.IdVeterinario,
                Nombres = v.Nombres,
                Apellidos = v.Apellidos,
                Especialidad = v.Especialidad,
                Telefono = v.Telefono,
                Activo = v.Activo
            })
            .ToList();
    }

    public int Guardar(Veterinario veterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        VeterinarioEntidad entidad = new() { Activo = true };
        AsignarCampos(entidad, veterinario);
        contexto.Veterinarios.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdVeterinario;
    }

    public void Actualizar(Veterinario veterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        VeterinarioEntidad? entidad = contexto.Veterinarios.Find(veterinario.IdVeterinario);
        if (entidad is null)
        {
            return;
        }

        AsignarCampos(entidad, veterinario);
        contexto.SaveChanges();
    }

    public void Desactivar(int idVeterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Veterinarios.Where(v => v.IdVeterinario == idVeterinario)
            .ExecuteUpdate(v => v.SetProperty(x => x.Activo, false));
    }

    private static void AsignarCampos(VeterinarioEntidad entidad, Veterinario veterinario)
    {
        entidad.Nombres = veterinario.Nombres.Trim();
        entidad.Apellidos = veterinario.Apellidos.Trim();
        entidad.Especialidad = veterinario.Especialidad.Trim();
        entidad.Telefono = veterinario.Telefono.Trim();
    }
}
