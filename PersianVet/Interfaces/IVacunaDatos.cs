using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para vacunas.
/// </summary>
public interface IVacunaDatos
{
    List<Vacuna> Listar(int? idMascota = null, bool soloProximas = false);
    int Guardar(Vacuna vacuna);
}

