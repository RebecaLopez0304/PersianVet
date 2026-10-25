using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
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

        return contexto.Mascotas.AsNoTracking()
            .Where(m => (!soloActivas || m.Activa)
                && (texto == ""
                    || m.Nombre.Contains(texto)
                    || m.Cliente.Nombres.Contains(texto)
                    || m.Cliente.Apellidos.Contains(texto)))
            .OrderByDescending(m => m.IdMascota)
            .Select(m => new Mascota
            {
                IdMascota = m.IdMascota,
                IdCliente = m.IdCliente,
                Propietario = m.Cliente.Nombres + " " + m.Cliente.Apellidos,
                Nombre = m.Nombre,
                Especie = m.Especie,
                Raza = m.Raza,
                Sexo = m.Sexo,
                FechaNacimiento = m.FechaNacimiento,
                Color = m.Color,
                Peso = m.Peso,
                Observaciones = m.Observaciones ?? "",
                Activa = m.Activa
            })
            .ToList();
    }

    public int Guardar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        MascotaEntidad entidad = new() { Activa = true };
        AsignarCampos(entidad, mascota);
        contexto.Mascotas.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdMascota;
    }

    public void Actualizar(Mascota mascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        MascotaEntidad? entidad = contexto.Mascotas.Find(mascota.IdMascota);
        if (entidad is null)
        {
            return;
        }

        AsignarCampos(entidad, mascota);
        contexto.SaveChanges();
    }

    public void Desactivar(int idMascota)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Mascotas.Where(m => m.IdMascota == idMascota)
            .ExecuteUpdate(m => m.SetProperty(x => x.Activa, false));
    }

    private static void AsignarCampos(MascotaEntidad entidad, Mascota mascota)
    {
        entidad.IdCliente = mascota.IdCliente;
        entidad.Nombre = mascota.Nombre.Trim();
        entidad.Especie = mascota.Especie.Trim();
        entidad.Raza = mascota.Raza.Trim();
        entidad.Sexo = mascota.Sexo;
        entidad.FechaNacimiento = mascota.FechaNacimiento.Date;
        entidad.Color = mascota.Color.Trim();
        entidad.Peso = mascota.Peso;
        entidad.Observaciones = mascota.Observaciones.Trim();
    }
}
