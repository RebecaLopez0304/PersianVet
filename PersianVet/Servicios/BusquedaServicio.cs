using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Busca un texto en clientes, mascotas y citas a la vez.
/// </summary>
public class BusquedaServicio
{
    private readonly IBusquedaDatos busquedaDatos;

    public BusquedaServicio(IBusquedaDatos busquedaDatos)
    {
        this.busquedaDatos = busquedaDatos;
    }

    public Task<List<ResultadoBusqueda>> BuscarAsync(string texto, CancellationToken cancelacion = default)
    {
        string limpio = texto.Trim();
        if (limpio.Length < 2)
        {
            return Task.FromResult(new List<ResultadoBusqueda>());
        }

        return busquedaDatos.BuscarAsync(limpio, cancelacion);
    }
}
