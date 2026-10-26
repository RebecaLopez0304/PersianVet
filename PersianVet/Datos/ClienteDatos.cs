using Microsoft.EntityFrameworkCore;
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
            .ToList();
    }

    public int Guardar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cliente);
        cliente.Activo = true;
        contexto.Clientes.Add(cliente);
        contexto.SaveChanges();
        return cliente.IdCliente;
    }

    public void Actualizar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cliente);
        contexto.Clientes.Update(cliente);
        // Modificar no cambia el estado activo/desactivado.
        contexto.Entry(cliente).Property(c => c.Activo).IsModified = false;
        contexto.SaveChanges();
    }

    public void Desactivar(int idCliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Clientes.Where(c => c.IdCliente == idCliente)
            .ExecuteUpdate(c => c.SetProperty(x => x.Activo, false));
    }

    private static void Limpiar(Cliente cliente)
    {
        cliente.Nombres = cliente.Nombres.Trim();
        cliente.Apellidos = cliente.Apellidos.Trim();
        cliente.Telefono = cliente.Telefono.Trim();
        cliente.Direccion = cliente.Direccion.Trim();
        cliente.Correo = cliente.Correo.Trim();
        cliente.FechaRegistro = cliente.FechaRegistro.Date;
    }
}
