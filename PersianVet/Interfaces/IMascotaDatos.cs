using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para mascotas.
/// </summary>
public interface IMascotaDatos
{
    List<Mascota> Listar(string filtro, bool soloActivas = true);
    int Guardar(Mascota mascota);
    void Actualizar(Mascota mascota);
    void Desactivar(int idMascota);
}
