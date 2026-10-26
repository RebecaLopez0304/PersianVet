using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de las citas.
/// </summary>
public interface ICitaDatos
{
    /// <summary>
    /// Obtiene las citas ordenadas por fecha, con la mascota, su dueño y el veterinario cargados.
    /// </summary>
    /// <param name="fecha">Día a consultar; <see langword="null"/> devuelve todas las citas.</param>
    /// <returns>Las citas encontradas.</returns>
    List<Cita> Listar(DateTime? fecha = null);

    /// <summary>
    /// Indica si el veterinario ya tiene otra cita activa en la misma fecha y hora.
    /// </summary>
    /// <param name="cita">Cita que se quiere guardar.</param>
    /// <returns><see langword="true"/> si existe un cruce de horario.</returns>
    bool ExisteCruce(Cita cita);

    /// <summary>
    /// Registra una cita nueva.
    /// </summary>
    /// <param name="cita">Datos de la cita.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Cita cita);

    /// <summary>
    /// Modifica los datos de una cita existente.
    /// </summary>
    /// <param name="cita">Datos de la cita, con su identificador.</param>
    void Actualizar(Cita cita);

    /// <summary>
    /// Marca una cita como cancelada; el registro se conserva.
    /// </summary>
    /// <param name="idCita">Identificador de la cita.</param>
    void Cancelar(int idCita);
}
