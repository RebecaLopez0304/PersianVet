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

    public List<Cita> Listar(DateTime? fecha = null)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include trae la mascota, su dueño y el veterinario para mostrar sus nombres.
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

    public bool ExisteCruce(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Citas.Any(c => c.IdVeterinario == cita.IdVeterinario
            && c.FechaHora == cita.FechaHora
            && c.Estado != "Cancelada"
            && c.IdCita != cita.IdCita);
    }

    public int Guardar(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cita);
        contexto.Citas.Add(cita);
        contexto.SaveChanges();
        return cita.IdCita;
    }

    public void Actualizar(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cita);
        contexto.Citas.Update(cita);
        contexto.SaveChanges();
    }

    public void Cancelar(int idCita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Citas.Where(c => c.IdCita == idCita)
            .ExecuteUpdate(c => c.SetProperty(x => x.Estado, "Cancelada"));
    }

    private static void Limpiar(Cita cita)
    {
        // Se guarda solo la cita; la mascota y el veterinario relacionados no se modifican.
        cita.Mascota = null!;
        cita.Veterinario = null!;
        cita.Motivo = cita.Motivo.Trim();
    }
}
