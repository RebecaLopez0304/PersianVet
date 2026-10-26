using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de citas mediante Entity Framework Core.
/// </summary>
public class CitaDatos : ICitaDatos
{
    private readonly ConexionBD conexionBD;

    public CitaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public List<Cita> Listar(DateTime? fecha = null)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include carga la mascota, su dueño y el veterinario que usan las propiedades Nombre* de Cita.
        IQueryable<Cita> consulta = contexto.Citas.AsNoTracking()
            .Include(c => c.Mascota).ThenInclude(m => m.Cliente)
            .Include(c => c.Veterinario);

        if (fecha.HasValue)
        {
            DateTime inicio = fecha.Value.Date;
            DateTime fin = inicio.AddDays(1);
            consulta = consulta.Where(c => c.FechaHora >= inicio && c.FechaHora < fin);
        }

        return consulta.OrderBy(c => c.FechaHora).ToList();
    }

    /// <inheritdoc/>
    public bool ExisteCruce(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Citas.Any(c => c.IdVeterinario == cita.IdVeterinario
            && c.FechaHora == cita.FechaHora
            && c.Estado != "Cancelada"
            && c.IdCita != cita.IdCita);
    }

    /// <inheritdoc/>
    public int Guardar(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cita);
        contexto.Citas.Add(cita);
        contexto.SaveChanges();
        return cita.IdCita;
    }

    /// <inheritdoc/>
    public void Actualizar(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cita);
        contexto.Citas.Update(cita);
        contexto.SaveChanges();
    }

    /// <inheritdoc/>
    public void Cancelar(int idCita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Citas.Where(c => c.IdCita == idCita)
            .ExecuteUpdate(c => c.SetProperty(x => x.Estado, "Cancelada"));
    }

    private static void Limpiar(Cita cita)
    {
        // Sin las relaciones cargadas, Update guarda solo la cita y no la mascota ni el veterinario.
        cita.Mascota = null!;
        cita.Veterinario = null!;
        cita.Motivo = cita.Motivo.Trim();
    }
}
