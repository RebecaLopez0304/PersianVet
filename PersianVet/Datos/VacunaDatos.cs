using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de vacunas mediante Entity Framework Core.
/// </summary>
public class VacunaDatos : IVacunaDatos
{
    private readonly ConexionBD conexionBD;

    public VacunaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Vacuna> Listar(int? idMascota = null, bool soloProximas = false)
    {
        DateTime hoy = DateTime.Today;
        DateTime limite = hoy.AddDays(30);
        using PersianVetContext contexto = conexionBD.CrearContexto();

        return contexto.Vacunas.AsNoTracking()
            .Where(v => (idMascota == null || v.IdMascota == idMascota)
                && (!soloProximas || (v.ProximaDosis >= hoy && v.ProximaDosis <= limite)))
            .OrderByDescending(v => v.FechaAplicacion)
            .Select(v => new Vacuna
            {
                IdVacuna = v.IdVacuna,
                IdMascota = v.IdMascota,
                Mascota = v.Mascota.Nombre,
                NombreVacuna = v.NombreVacuna,
                FechaAplicacion = v.FechaAplicacion,
                ProximaDosis = v.ProximaDosis,
                Lote = v.Lote
            })
            .ToList();
    }

    public int Guardar(Vacuna vacuna)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        VacunaEntidad entidad = new()
        {
            IdMascota = vacuna.IdMascota,
            NombreVacuna = vacuna.NombreVacuna.Trim(),
            FechaAplicacion = vacuna.FechaAplicacion.Date,
            ProximaDosis = vacuna.ProximaDosis?.Date,
            Lote = vacuna.Lote.Trim()
        };
        contexto.Vacunas.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdVacuna;
    }
}
