using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de las vacunas.
/// </summary>
public interface IVacunaDatos
{
    /// <summary>
    /// Obtiene las vacunas aplicadas, de la más reciente a la más antigua.
    /// </summary>
    /// <param name="idMascota">Mascota a consultar; <see langword="null"/> devuelve todas.</param>
    /// <param name="soloProximas"><see langword="true"/> para devolver solo las que vencen en los próximos 30 días.</param>
    /// <returns>Las vacunas encontradas, con su mascota cargada.</returns>
    List<Vacuna> Listar(int? idMascota = null, bool soloProximas = false);

    /// <summary>
    /// Registra una vacuna aplicada.
    /// </summary>
    /// <param name="vacuna">Datos de la vacuna.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Vacuna vacuna);
}
