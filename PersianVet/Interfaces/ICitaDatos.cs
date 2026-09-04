using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para citas.
/// </summary>
public interface ICitaDatos
{
    List<Cita> Listar(DateTime? fecha = null);
    bool ExisteCruce(Cita cita);
    int Guardar(Cita cita);
    void Actualizar(Cita cita);
    void Cancelar(int idCita);
}

