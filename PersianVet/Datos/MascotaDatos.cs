using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de mascotas mediante Entity Framework Core.
/// </summary>
public class MascotaDatos : IMascotaDatos
{
    private readonly ConexionBD conexionBD;

    public MascotaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public List<Mascota> Listar(string filtro, bool soloActivas = true)
    {
        string texto = filtro.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include carga el cliente que usa la propiedad Propietario.
        return contexto.Mascotas.AsNoTracking()
            .Include(m => m.Cliente)
            .Where(m => (!soloActivas || m.Activa)
                && (texto == ""
                    || m.Nombre.Contains(texto)
                    || m.Cliente.Nombres.Contains(texto)
                    || m.Cliente.Apellidos.Contains(texto)))
            .OrderByDescending(m => m.IdMascota)
            .ToList();
    }

    /// <inheritdoc/>
    public int Guardar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(mascota);
        mascota.Activa = true;
        contexto.Mascotas.Add(mascota);
        contexto.SaveChanges();
        return mascota.IdMascota;
    }

    /// <inheritdoc/>
    public void Actualizar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(mascota);
        contexto.Mascotas.Update(mascota);
        // El estado activo solo cambia con Desactivar; Update no debe sobrescribirlo.
        contexto.Entry(mascota).Property(m => m.Activa).IsModified = false;
        contexto.SaveChanges();
    }

    /// <inheritdoc/>
    public void Desactivar(int idMascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Mascotas.Where(m => m.IdMascota == idMascota)
            .ExecuteUpdate(m => m.SetProperty(x => x.Activa, false));
    }

    private static void Limpiar(Mascota mascota)
    {
        // Sin el cliente cargado, Update guarda solo la mascota y no modifica al cliente.
        mascota.Cliente = null!;
        mascota.Nombre = mascota.Nombre.Trim();
        mascota.Especie = mascota.Especie.Trim();
        mascota.Raza = mascota.Raza.Trim();
        mascota.FechaNacimiento = mascota.FechaNacimiento.Date;
        mascota.Color = mascota.Color.Trim();
        mascota.Observaciones = (mascota.Observaciones ?? string.Empty).Trim();
    }
}
