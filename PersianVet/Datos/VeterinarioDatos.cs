using Microsoft.EntityFrameworkCore;
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
            .ToList();
    }

    public int Guardar(Veterinario veterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(veterinario);
        veterinario.Activo = true;
        contexto.Veterinarios.Add(veterinario);
        contexto.SaveChanges();
        return veterinario.IdVeterinario;
    }

    public void Actualizar(Veterinario veterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(veterinario);
        contexto.Veterinarios.Update(veterinario);
        // Modificar no cambia el estado activo/desactivado.
        contexto.Entry(veterinario).Property(v => v.Activo).IsModified = false;
        contexto.SaveChanges();
    }

    public void Desactivar(int idVeterinario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Veterinarios.Where(v => v.IdVeterinario == idVeterinario)
            .ExecuteUpdate(v => v.SetProperty(x => x.Activo, false));
    }

    private static void Limpiar(Veterinario veterinario)
    {
        veterinario.Nombres = veterinario.Nombres.Trim();
        veterinario.Apellidos = veterinario.Apellidos.Trim();
        veterinario.Especialidad = veterinario.Especialidad.Trim();
        veterinario.Telefono = veterinario.Telefono.Trim();
    }
}
