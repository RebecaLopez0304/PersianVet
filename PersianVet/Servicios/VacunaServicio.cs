using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones de vacunas.
/// </summary>
public class VacunaServicio
{
    private readonly IVacunaDatos vacunaDatos;

    public VacunaServicio(IVacunaDatos vacunaDatos)
    {
        this.vacunaDatos = vacunaDatos;
    }

    /// <inheritdoc cref="IVacunaDatos.Listar(int?, bool)"/>
    public List<Vacuna> Listar(int? idMascota = null, bool soloProximas = false)
    {
        return vacunaDatos.Listar(idMascota, soloProximas);
    }

    /// <summary>
    /// Valida la mascota, el lote y que las fechas sean coherentes, y registra la vacuna.
    /// </summary>
    /// <param name="vacuna">Datos de la vacuna.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Vacuna vacuna)
    {
        if (vacuna.IdMascota <= 0)
        {
            return ResultadoOperacion.Error("Seleccione una mascota.");
        }

        if (string.IsNullOrWhiteSpace(vacuna.NombreVacuna) || string.IsNullOrWhiteSpace(vacuna.Lote))
        {
            return ResultadoOperacion.Error("Escriba el nombre de la vacuna y el lote.");
        }

        if (vacuna.FechaAplicacion.Date > DateTime.Today)
        {
            return ResultadoOperacion.Error("La fecha de aplicación no puede estar en el futuro.");
        }

        if (vacuna.ProximaDosis.HasValue && vacuna.ProximaDosis.Value.Date < vacuna.FechaAplicacion.Date)
        {
            return ResultadoOperacion.Error("La próxima dosis no puede ser anterior a la aplicación.");
        }

        vacuna.IdVacuna = vacunaDatos.Guardar(vacuna);
        return ResultadoOperacion.Correcto("La vacuna se guardó correctamente.");
    }
}

