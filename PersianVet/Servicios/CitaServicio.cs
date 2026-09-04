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

    public List<Cita> Listar(DateTime? fecha = null)
    {
        return citaDatos.Listar(fecha);
    }

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

