using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para consultas médicas.
/// </summary>
public interface IConsultaDatos
{
    List<Consulta> ListarPorMascota(int idMascota);
    int GuardarConTratamientos(Consulta consulta);
}

