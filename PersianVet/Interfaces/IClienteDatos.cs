using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de los clientes.
/// </summary>
public interface IClienteDatos
{
    /// <summary>
    /// Obtiene los clientes cuyo nombre, apellido, teléfono o correo contienen el filtro.
    /// </summary>
    /// <param name="filtro">Texto a buscar; vacío devuelve todos.</param>
    /// <param name="soloActivos"><see langword="true"/> para excluir los clientes desactivados.</param>
    /// <returns>Los clientes encontrados, del más reciente al más antiguo.</returns>
    List<Cliente> Listar(string filtro, bool soloActivos = true);

    /// <summary>
    /// Registra un cliente nuevo como activo.
    /// </summary>
    /// <param name="cliente">Datos del cliente.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Cliente cliente);

    /// <summary>
    /// Modifica los datos de un cliente sin cambiar su estado activo.
    /// </summary>
    /// <param name="cliente">Datos del cliente, con su identificador.</param>
    void Actualizar(Cliente cliente);

    /// <summary>
    /// Desactiva un cliente; el registro se conserva para mantener su historial.
    /// </summary>
    /// <param name="idCliente">Identificador del cliente.</param>
    void Desactivar(int idCliente);
}
