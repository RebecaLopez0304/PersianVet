using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
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
        IQueryable<CitaEntidad> consulta = contexto.Citas.AsNoTracking();

        if (fecha.HasValue)
        {
            DateTime inicio = fecha.Value.Date;
            DateTime fin = inicio.AddDays(1);
            consulta = consulta.Where(c => c.FechaHora >= inicio && c.FechaHora < fin);
        }

        return consulta
            .OrderBy(c => c.FechaHora)
            .Select(c => new Cita
            {
                IdCita = c.IdCita,
                IdMascota = c.IdMascota,
                IdVeterinario = c.IdVeterinario,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                Estado = c.Estado,
                Mascota = c.Mascota.Nombre,
                Cliente = c.Mascota.Cliente.Nombres + " " + c.Mascota.Cliente.Apellidos,
                Veterinario = c.Veterinario.Nombres + " " + c.Veterinario.Apellidos
            })
            .ToList();
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
        CitaEntidad entidad = new();
        AsignarCampos(entidad, cita);
        contexto.Citas.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdCita;
    }

    public void Actualizar(Cita cita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        CitaEntidad? entidad = contexto.Citas.Find(cita.IdCita);
        if (entidad is null)
        {
            return;
        }

        AsignarCampos(entidad, cita);
        contexto.SaveChanges();
    }

    public void Cancelar(int idCita)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Citas.Where(c => c.IdCita == idCita)
            .ExecuteUpdate(c => c.SetProperty(x => x.Estado, "Cancelada"));
    }

    private static void AsignarCampos(CitaEntidad entidad, Cita cita)
    {
        entidad.IdMascota = cita.IdMascota;
        entidad.IdVeterinario = cita.IdVeterinario;
        entidad.FechaHora = cita.FechaHora;
        entidad.Motivo = cita.Motivo.Trim();
        entidad.Estado = cita.Estado;
    }
}
