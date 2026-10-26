using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de los veterinarios.
/// </summary>
public interface IVeterinarioDatos
{
    /// <summary>
    /// Obtiene los veterinarios cuyo nombre, apellido o especialidad contienen el filtro.
    /// </summary>
    /// <param name="filtro">Texto a buscar; vacío devuelve todos.</param>
    /// <param name="soloActivos"><see langword="true"/> para excluir los veterinarios desactivados.</param>
    /// <returns>Los veterinarios encontrados, del más reciente al más antiguo.</returns>
    List<Veterinario> Listar(string filtro, bool soloActivos = true);

    /// <summary>
    /// Registra un veterinario nuevo como activo.
    /// </summary>
    /// <param name="veterinario">Datos del veterinario.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Veterinario veterinario);

    /// <summary>
    /// Modifica los datos de un veterinario sin cambiar su estado activo.
    /// </summary>
    /// <param name="veterinario">Datos del veterinario, con su identificador.</param>
    void Actualizar(Veterinario veterinario);

    /// <summary>
    /// Desactiva un veterinario; el registro se conserva para mantener su historial.
    /// </summary>
    /// <param name="idVeterinario">Identificador del veterinario.</param>
    void Desactivar(int idVeterinario);
}
