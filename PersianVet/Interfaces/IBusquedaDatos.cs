using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define la búsqueda global sobre clientes, mascotas y citas.
/// </summary>
public interface IBusquedaDatos
{
    Task<List<ResultadoBusqueda>> BuscarAsync(string texto, CancellationToken cancelacion);
}
