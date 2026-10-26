using PersianVet.Interfaces;
using PersianVet.Modelos;

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
    /// Valida la cita, que no sea en el pasado y que el veterinario no tenga otra a la misma hora, y la registra.
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

        if (citaDatos.ExisteCruce(cita))
        {
            return ResultadoOperacion.Error("El veterinario ya tiene una cita en esa fecha y hora.");
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

        if (citaDatos.ExisteCruce(cita))
        {
            return ResultadoOperacion.Error("El veterinario ya tiene una cita en esa fecha y hora.");
        }

        citaDatos.Actualizar(cita);
        return ResultadoOperacion.Correcto("La cita se reprogramó correctamente.");
    }

    /// <inheritdoc cref="ICitaDatos.Cancelar(int)"/>
    public void Cancelar(int idCita)
    {
        citaDatos.Cancelar(idCita);
    }

    private static ResultadoOperacion Validar(Cita cita)
    {
        if (cita.IdMascota <= 0 || cita.IdVeterinario <= 0)
        {
            return ResultadoOperacion.Error("Seleccione la mascota y el veterinario.");
        }

        if (cita.FechaHora < DateTime.Now.AddMinutes(-1))
        {
            return ResultadoOperacion.Error("La cita no puede programarse en una fecha pasada.");
        }

        if (string.IsNullOrWhiteSpace(cita.Motivo))
        {
            return ResultadoOperacion.Error("Escriba el motivo de la cita.");
        }

        return ResultadoOperacion.Correcto(string.Empty);
    }
}

