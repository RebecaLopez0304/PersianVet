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
                Titulo = c.Nombres + " " + c.Apellidos,
                Detalle = c.Telefono + " | " + c.Correo
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
                Titulo = m.Nombre,
                Detalle = m.Especie + " " + m.Raza + " | Dueño: " + m.Cliente.Nombres + " " + m.Cliente.Apellidos
            })
            .ToListAsync(cancelacion);
    }

    private async Task<List<ResultadoBusqueda>> BuscarCitasAsync(string texto, CancellationToken cancelacion)
    {
        DateTime hoy = DateTime.Today;
        await using PersianVetContext contexto = conexionBD.CrearContexto();

        // La fecha se formatea en memoria porque ToString("dd/MM HH:mm") no se traduce a SQL.
        var citas = await contexto.Citas.AsNoTracking()
            .Where(c => c.Estado != "Cancelada" && c.FechaHora >= hoy
                && (c.Motivo.Contains(texto) || c.Mascota.Nombre.Contains(texto)))
            .OrderBy(c => c.FechaHora)
            .Take(MaximoPorTipo)
            .Select(c => new { c.IdCita, Mascota = c.Mascota.Nombre, c.Motivo, c.FechaHora, c.Estado })
            .ToListAsync(cancelacion);

        return citas.Select(c => new ResultadoBusqueda
        {
            Id = c.IdCita,
            Tipo = "Cita",
            Titulo = $"{c.Mascota} - {c.Motivo}",
            Detalle = $"{c.FechaHora:dd/MM/yyyy HH:mm} | {c.Estado}"
        }).ToList();
    }
}
