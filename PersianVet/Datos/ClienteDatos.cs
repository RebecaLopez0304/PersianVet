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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public string? ObtenerNombreConCorreo(string correo, int idClienteExcluido)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Clientes.AsNoTracking()
            .Where(c => c.Correo == correo && c.IdCliente != idClienteExcluido)
            .Select(c => c.Nombres + " " + c.Apellidos)
            .FirstOrDefault();
    }

    /// <inheritdoc/>
    public int Guardar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cliente);
        cliente.Activo = true;
        contexto.Clientes.Add(cliente);
        contexto.SaveChanges();
        return cliente.IdCliente;
    }

    /// <inheritdoc/>
    public void Actualizar(Cliente cliente)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Limpiar(cliente);
        contexto.Clientes.Update(cliente);
        // El estado activo solo cambia con Desactivar; Update no debe sobrescribirlo.
        contexto.Entry(cliente).Property(c => c.Activo).IsModified = false;
        contexto.SaveChanges();
    }

    /// <inheritdoc/>
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
