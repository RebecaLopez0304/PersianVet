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

    public List<Mascota> Listar(string filtro, bool soloActivas = true)
    {
        string texto = filtro.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include trae el cliente para poder mostrar el nombre del propietario.
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

    public int Guardar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(mascota);
        mascota.Activa = true;
        contexto.Mascotas.Add(mascota);
        contexto.SaveChanges();
        return mascota.IdMascota;
    }

    public void Actualizar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(mascota);
        contexto.Mascotas.Update(mascota);
        // Modificar no cambia el estado activo/desactivado.
        contexto.Entry(mascota).Property(m => m.Activa).IsModified = false;
        contexto.SaveChanges();
    }

    public void Desactivar(int idMascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Mascotas.Where(m => m.IdMascota == idMascota)
            .ExecuteUpdate(m => m.SetProperty(x => x.Activa, false));
    }

    private static void Limpiar(Mascota mascota)
    {
        // Se guarda solo la mascota; el cliente relacionado no se modifica.
        mascota.Cliente = null!;
        mascota.Nombre = mascota.Nombre.Trim();
        mascota.Especie = mascota.Especie.Trim();
        mascota.Raza = mascota.Raza.Trim();
        mascota.FechaNacimiento = mascota.FechaNacimiento.Date;
        mascota.Color = mascota.Color.Trim();
        mascota.Observaciones = (mascota.Observaciones ?? string.Empty).Trim();
    }
}
