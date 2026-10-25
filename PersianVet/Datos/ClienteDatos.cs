using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones de clientes mediante Entity Framework Core.
/// </summary>
public class ClienteDatos : IClienteDatos
{
    private readonly ConexionBD conexionBD;

    public ClienteDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    public List<Cliente> Listar(string filtro, bool soloActivos = true)
    {
        string texto = filtro.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();

        return contexto.Clientes.AsNoTracking()
            .Where(c => (!soloActivos || c.Activo)
                && (texto == ""
                    || c.Nombres.Contains(texto)
                    || c.Apellidos.Contains(texto)
                    || c.Telefono.Contains(texto)
                    || c.Correo.Contains(texto)))
            .OrderByDescending(c => c.IdCliente)
            .Select(c => new Cliente
            {
                IdCliente = c.IdCliente,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                Correo = c.Correo,
                FechaRegistro = c.FechaRegistro,
                Activo = c.Activo
            })
            .ToList();
    }

    public int Guardar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        ClienteEntidad entidad = new() { Activo = true };
        AsignarCampos(entidad, cliente);
        contexto.Clientes.Add(entidad);
        contexto.SaveChanges();
        return entidad.IdCliente;
    }

    public void Actualizar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        ClienteEntidad? entidad = contexto.Clientes.Find(cliente.IdCliente);
        if (entidad is null)
        {
            return;
        }

        AsignarCampos(entidad, cliente);
        contexto.SaveChanges();
    }

    public void Desactivar(int idCliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Clientes.Where(c => c.IdCliente == idCliente)
            .ExecuteUpdate(c => c.SetProperty(x => x.Activo, false));
    }

    private static void AsignarCampos(ClienteEntidad entidad, Cliente cliente)
    {
        entidad.Nombres = cliente.Nombres.Trim();
        entidad.Apellidos = cliente.Apellidos.Trim();
        entidad.Telefono = cliente.Telefono.Trim();
        entidad.Direccion = cliente.Direccion.Trim();
        entidad.Correo = cliente.Correo.Trim();
        entidad.FechaRegistro = cliente.FechaRegistro.Date;
    }
}
