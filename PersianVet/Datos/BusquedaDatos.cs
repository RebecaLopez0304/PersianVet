using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Búsqueda global con LINQ: consulta clientes, mascotas y citas al mismo tiempo.
/// </summary>
public class BusquedaDatos : IBusquedaDatos
{
    private const int MaximoPorTipo = 10;
    private readonly ConexionBD conexionBD;

    public BusquedaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public async Task<List<ResultadoBusqueda>> BuscarAsync(string texto, CancellationToken cancelacion)
    {
        // DbContext no admite consultas simultáneas: cada búsqueda paralela usa su propio contexto.
        Task<List<ResultadoBusqueda>> clientes = BuscarClientesAsync(texto, cancelacion);
        Task<List<ResultadoBusqueda>> mascotas = BuscarMascotasAsync(texto, cancelacion);
        Task<List<ResultadoBusqueda>> citas = BuscarCitasAsync(texto, cancelacion);

        await Task.WhenAll(clientes, mascotas, citas);
        return [.. clientes.Result, .. mascotas.Result, .. citas.Result];
    }

    private async Task<List<ResultadoBusqueda>> BuscarClientesAsync(string texto, CancellationToken cancelacion)
    {
        await using PersianVetContext contexto = conexionBD.CrearContexto();
        return await contexto.Clientes.AsNoTracking()
            .Where(c => c.Activo
                && (c.Nombres.Contains(texto) || c.Apellidos.Contains(texto)
                    || c.Telefono.Contains(texto) || c.Correo.Contains(texto)))
            .OrderBy(c => c.Nombres)
            .Take(MaximoPorTipo)
            .Select(c => new ResultadoBusqueda
            {
                Id = c.IdCliente,
                Tipo = "Cliente",
                Nombre = c.Nombres + " " + c.Apellidos,
                Descripcion = c.Correo,
                Telefono = c.Telefono
            })
            .ToListAsync(cancelacion);
    }

    private async Task<List<ResultadoBusqueda>> BuscarMascotasAsync(string texto, CancellationToken cancelacion)
    {
        await using PersianVetContext contexto = conexionBD.CrearContexto();
        return await contexto.Mascotas.AsNoTracking()
            .Where(m => m.Activa
                && (m.Nombre.Contains(texto) || m.Raza.Contains(texto)
                    || m.Cliente.Nombres.Contains(texto) || m.Cliente.Apellidos.Contains(texto)))
            .OrderBy(m => m.Nombre)
            .Take(MaximoPorTipo)
            .Select(m => new ResultadoBusqueda
            {
                Id = m.IdMascota,
                Tipo = "Mascota",
                Nombre = m.Nombre,
                Descripcion = m.Raza + " (" + m.Especie + ")",
                Propietario = m.Cliente.Nombres + " " + m.Cliente.Apellidos,
                Telefono = m.Cliente.Telefono
            })
            .ToListAsync(cancelacion);
    }

    private async Task<List<ResultadoBusqueda>> BuscarCitasAsync(string texto, CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        return await contexto.Citas.AsNoTracking()
            .Where(c => c.Estado != "Cancelada" && c.FechaHora >= hoy
                && (c.Motivo.Contains(texto) || c.Mascota.Nombre.Contains(texto)))
            .OrderBy(c => c.FechaHora)
            .Take(MaximoPorTipo)
            .Select(c => new ResultadoBusqueda
            {
                Id = c.IdCita,
                Tipo = "Cita",
                Nombre = c.Mascota.Nombre,
                Descripcion = c.Motivo,
                Propietario = c.Mascota.Cliente.Nombres + " " + c.Mascota.Cliente.Apellidos,
                Telefono = c.Mascota.Cliente.Telefono,
                Fecha = c.FechaHora,
                Estado = c.Estado
            })
            .ToListAsync(cancelacion);
    }
}
