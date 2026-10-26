using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones de acceso a datos de las mascotas.
/// </summary>
public interface IMascotaDatos
{
    /// <summary>
    /// Obtiene las mascotas cuyo nombre o el de su propietario contienen el filtro.
    /// </summary>
    /// <param name="filtro">Texto a buscar; vacío devuelve todas.</param>
    /// <param name="soloActivas"><see langword="true"/> para excluir las mascotas desactivadas.</param>
    /// <returns>Las mascotas encontradas, con su propietario cargado.</returns>
    List<Mascota> Listar(string filtro, bool soloActivas = true);

    /// <summary>
    /// Registra una mascota nueva como activa.
    /// </summary>
    /// <param name="mascota">Datos de la mascota.</param>
    /// <returns>El identificador asignado por la base de datos.</returns>
    int Guardar(Mascota mascota);

    /// <summary>
    /// Modifica los datos de una mascota sin cambiar su estado activo.
    /// </summary>
    /// <param name="mascota">Datos de la mascota, con su identificador.</param>
    void Actualizar(Mascota mascota);

    /// <summary>
    /// Desactiva una mascota; el registro se conserva para mantener su historial.
    /// </summary>
    /// <param name="idMascota">Identificador de la mascota.</param>
    void Desactivar(int idMascota);
}
