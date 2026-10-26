using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos del catálogo de tarifas y de su historial de precios.
/// </summary>
public interface ITarifaDatos
{
    /// <summary>
    /// Obtiene las tarifas ordenadas por categoría y nombre, con su historial cargado.
    /// </summary>
    /// <param name="tipo">Categoría a filtrar; <see langword="null"/> devuelve todas.</param>
    /// <param name="soloActivas"><see langword="true"/> para excluir las tarifas desactivadas.</param>
    /// <returns>Las tarifas encontradas.</returns>
    List<Tarifa> Listar(string? tipo = null, bool soloActivas = true);

    /// <summary>
    /// Indica si ya existe otra tarifa con el mismo nombre.
    /// </summary>
    /// <param name="tarifa">Tarifa que se quiere guardar.</param>
    /// <returns><see langword="true"/> si el nombre ya está en uso.</returns>
    bool ExisteNombre(Tarifa tarifa);

    /// <summary>
    /// Registra una tarifa nueva como activa y guarda su precio inicial en el historial.
    /// </summary>
    /// <param name="tarifa">Datos de la tarifa.</param>
    /// <param name="usuario">Usuario que la registra.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Tarifa tarifa, string usuario);

    /// <summary>
    /// Modifica el nombre, la categoría y el precio de una tarifa sin cambiar su estado activo.
    /// Si el precio cambia, agrega el cambio al historial en la misma transacción.
    /// </summary>
    /// <param name="tarifa">Datos de la tarifa, con su identificador.</param>
    /// <param name="usuario">Usuario que hace el cambio.</param>
    /// <returns>El precio que tenía la tarifa antes del cambio.</returns>
    decimal Actualizar(Tarifa tarifa, string usuario);

    /// <summary>
    /// Desactiva una tarifa para que ya no se ofrezca al registrar; el historial conserva sus precios.
    /// </summary>
    /// <param name="idTarifa">Identificador de la tarifa.</param>
    void Desactivar(int idTarifa);

    /// <summary>
    /// Obtiene los cambios de precio de una tarifa, del más reciente al más antiguo.
    /// </summary>
    /// <param name="idTarifa">Identificador de la tarifa.</param>
    /// <returns>El historial de precios.</returns>
    List<HistorialPrecio> ListarHistorial(int idTarifa);

    /// <summary>
    /// Obtiene los últimos registros donde se cobró la tarifa, con el precio que se guardó en cada uno.
    /// </summary>
    /// <param name="tarifa">Tarifa a revisar (se busca por su nombre y categoría).</param>
    /// <param name="maximo">Cantidad máxima de registros.</param>
    /// <returns>Los cobros, del más reciente al más antiguo.</returns>
    List<CobroTarifa> ListarCobros(Tarifa tarifa, int maximo);
}
