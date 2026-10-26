using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define la búsqueda global sobre clientes, mascotas y citas.
/// </summary>
public interface IBusquedaDatos
{
    /// <summary>
    /// Busca el texto en clientes, mascotas y citas futuras.
    /// </summary>
    /// <param name="texto">Texto a buscar.</param>
    /// <param name="cancelacion">Permite cancelar la búsqueda.</param>
    /// <returns>Las coincidencias de los tres tipos, como máximo diez por tipo.</returns>
    Task<List<ResultadoBusqueda>> BuscarAsync(string texto, CancellationToken cancelacion);
}
