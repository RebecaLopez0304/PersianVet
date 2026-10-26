using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

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
        DateTime aplicacion = vacuna.FechaAplicacion.Date;
        Validador validador = new Validador()
            .Seleccionado(vacuna.IdMascota, "Mascota")
            .Seleccionado(vacuna.NombreVacuna, "Vacuna")
            .Regla(aplicacion <= DateTime.Today, "Fecha de aplicación",
                $"{aplicacion:dd/MM/yyyy} está en el futuro; registre la vacuna el día en que se aplica.")
            .Requerido(vacuna.Lote, "Lote")
            .LongitudMaxima(vacuna.Lote, 60, "Lote")
            .Regla(vacuna.ProximaDosis is null || vacuna.ProximaDosis.Value.Date >= aplicacion, "Próxima dosis",
                $"{vacuna.ProximaDosis:dd/MM/yyyy} es anterior a la fecha de aplicación ({aplicacion:dd/MM/yyyy}).");

        if (!validador.EsValido)
        {
            return validador.Resultado();
        }

        vacuna.IdVacuna = vacunaDatos.Guardar(vacuna);
        return ResultadoOperacion.Correcto("La vacuna se guardó correctamente.");
    }
}

