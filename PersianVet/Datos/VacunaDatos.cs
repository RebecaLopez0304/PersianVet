using Microsoft.EntityFrameworkCore;
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
            .Include(v => v.Mascota)
            .Where(v => (idMascota == null || v.IdMascota == idMascota)
                && (!soloProximas || (v.ProximaDosis >= hoy && v.ProximaDosis <= limite)))
            .OrderByDescending(v => v.FechaAplicacion)
            .ToList();
    }

    public int Guardar(Vacuna vacuna)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        vacuna.Mascota = null!;
        vacuna.NombreVacuna = vacuna.NombreVacuna.Trim();
        vacuna.FechaAplicacion = vacuna.FechaAplicacion.Date;
        vacuna.ProximaDosis = vacuna.ProximaDosis?.Date;
        vacuna.Lote = vacuna.Lote.Trim();
        contexto.Vacunas.Add(vacuna);
        contexto.SaveChanges();
        return vacuna.IdVacuna;
    }
}
