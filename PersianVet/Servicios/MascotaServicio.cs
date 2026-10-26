using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones de mascotas.
/// </summary>
public class MascotaServicio
{
    private readonly IMascotaDatos mascotaDatos;

    public MascotaServicio(IMascotaDatos mascotaDatos)
    {
        this.mascotaDatos = mascotaDatos;
    }

    /// <inheritdoc cref="IMascotaDatos.Listar(string, bool)"/>
    public List<Mascota> Listar(string filtro, bool soloActivas = true)
    {
        return mascotaDatos.Listar(filtro, soloActivas);
    }

    /// <summary>
    /// Valida el propietario, los datos básicos, la fecha de nacimiento y el peso, y registra la mascota.
    /// </summary>
    /// <param name="mascota">Datos de la mascota.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Mascota mascota)
    {
        ResultadoOperacion validacion = Validar(mascota);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        mascota.IdMascota = mascotaDatos.Guardar(mascota);
        return ResultadoOperacion.Correcto("La mascota se guardó correctamente.");
    }

    /// <summary>
    /// Valida los datos y modifica una mascota existente.
    /// </summary>
    /// <param name="mascota">Datos de la mascota, con su identificador.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Actualizar(Mascota mascota)
    {
        if (mascota.IdMascota <= 0)
        {
            return ResultadoOperacion.Error("Seleccione una mascota de la lista.");
        }

        ResultadoOperacion validacion = Validar(mascota);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        mascotaDatos.Actualizar(mascota);
        return ResultadoOperacion.Correcto("La mascota se modificó correctamente.");
    }

    /// <inheritdoc cref="IMascotaDatos.Desactivar(int)"/>
    public void Desactivar(int idMascota)
    {
        mascotaDatos.Desactivar(idMascota);
    }

    private static ResultadoOperacion Validar(Mascota mascota)
    {
        if (mascota.IdCliente <= 0)
        {
            return ResultadoOperacion.Error("Seleccione el propietario de la mascota.");
        }

        if (string.IsNullOrWhiteSpace(mascota.Nombre) || string.IsNullOrWhiteSpace(mascota.Especie))
        {
            return ResultadoOperacion.Error("Escriba el nombre y la especie de la mascota.");
        }

        if (string.IsNullOrWhiteSpace(mascota.Raza) || string.IsNullOrWhiteSpace(mascota.Sexo))
        {
            return ResultadoOperacion.Error("Escriba la raza y seleccione el sexo de la mascota.");
        }

        if (mascota.FechaNacimiento.Date > DateTime.Today)
        {
            return ResultadoOperacion.Error("La fecha de nacimiento no puede estar en el futuro.");
        }

        if (mascota.Peso <= 0)
        {
            return ResultadoOperacion.Error("El peso debe ser mayor que cero.");
        }

        return ResultadoOperacion.Correcto(string.Empty);
    }
}
