using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de las consultas médicas.
/// </summary>
public interface IConsultaDatos
{
    /// <summary>
    /// Obtiene el historial de consultas de una mascota, de la más reciente a la más antigua.
    /// </summary>
    /// <param name="idMascota">Identificador de la mascota.</param>
    /// <returns>Las consultas con su veterinario y sus tratamientos cargados.</returns>
    List<Consulta> ListarPorMascota(int idMascota);

    /// <summary>
    /// Registra una consulta junto con sus tratamientos en una sola transacción.
    /// </summary>
    /// <param name="consulta">Consulta con la lista de tratamientos.</param>
    /// <returns>El identificador asignado a la consulta.</returns>
    int GuardarConTratamientos(Consulta consulta);
}
