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

    /// <summary>
    /// Busca el texto en clientes, mascotas y citas; con menos de dos letras devuelve una lista vacía sin consultar la base.
    /// </summary>
    /// <param name="texto">Texto a buscar.</param>
    /// <param name="cancelacion">Permite cancelar la búsqueda.</param>
    /// <returns>Las coincidencias encontradas.</returns>
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
