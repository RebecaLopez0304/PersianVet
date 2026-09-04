using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para veterinarios.
/// </summary>
public interface IVeterinarioDatos
{
    List<Veterinario> Listar(string filtro, bool soloActivos = true);
    int Guardar(Veterinario veterinario);
    void Actualizar(Veterinario veterinario);
    void Desactivar(int idVeterinario);
}
