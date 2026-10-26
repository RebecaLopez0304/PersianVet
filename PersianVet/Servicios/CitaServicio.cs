using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones de citas.
/// </summary>
public class CitaServicio
{
    private readonly ICitaDatos citaDatos;

    public CitaServicio(ICitaDatos citaDatos)
    {
        this.citaDatos = citaDatos;
    }

    /// <inheritdoc cref="ICitaDatos.Listar(DateTime?)"/>
    public List<Cita> Listar(DateTime? fecha = null)
    {
        return citaDatos.Listar(fecha);
    }

    /// <summary>
    /// Valida la cita y su servicio, que no sea en el pasado y que el veterinario no tenga otra a la misma hora, y la registra.
    /// </summary>
    /// <param name="cita">Datos de la cita.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Cita cita)
    {
        ResultadoOperacion validacion = Validar(cita);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        cita.IdCita = citaDatos.Guardar(cita);
        return ResultadoOperacion.Correcto("La cita se programó correctamente.");
    }

    /// <summary>
    /// Aplica las mismas validaciones que al programar y modifica una cita existente.
    /// </summary>
    /// <param name="cita">Datos de la cita, con su identificador.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Actualizar(Cita cita)
    {
        if (cita.IdCita <= 0)
        {
            return ResultadoOperacion.Error("Seleccione una cita de la lista.");
        }

        ResultadoOperacion validacion = Validar(cita);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        citaDatos.Actualizar(cita);
        return ResultadoOperacion.Correcto("La cita se reprogramó correctamente.");
    }

    /// <inheritdoc cref="ICitaDatos.Cancelar(int)"/>
    public void Cancelar(int idCita)
    {
        citaDatos.Cancelar(idCita);
    }

    private ResultadoOperacion Validar(Cita cita)
    {
        DateTime ahora = DateTime.Now;
        Validador validador = new Validador()
            .Seleccionado(cita.IdMascota, "Mascota")
            .Seleccionado(cita.IdVeterinario, "Veterinario")
            .Seleccionado(cita.Servicio, "Servicio")
            .Regla(cita.FechaHora >= ahora.AddMinutes(-1), "Fecha y hora",
                $"{cita.FechaHora:dd/MM/yyyy HH:mm} ya pasó; elija una fecha y hora posterior a {ahora:dd/MM/yyyy HH:mm}.")
            .Seleccionado(cita.Estado, "Estado")
            .Requerido(cita.Motivo, "Motivo")
            .LongitudMaxima(cita.Motivo, 200, "Motivo");

        // El cruce de horarios se revisa solo si lo demás es válido, para no consultar la base sin necesidad.
        if (validador.EsValido)
        {
            validador.Regla(!citaDatos.ExisteCruce(cita), "Horario",
                $"el veterinario seleccionado ya tiene una cita activa el {cita.FechaHora:dd/MM/yyyy} a las " +
                $"{cita.FechaHora:HH:mm}; elija otra hora u otro veterinario.");
        }

        return validador.Resultado();
    }
}
