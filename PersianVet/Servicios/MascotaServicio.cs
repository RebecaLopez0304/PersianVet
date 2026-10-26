using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

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
        DateTime hoy = DateTime.Today;
        DateTime nacimiento = mascota.FechaNacimiento.Date;

        return new Validador()
            .Seleccionado(mascota.IdCliente, "Propietario")
            .Requerido(mascota.Nombre, "Nombre")
            .LongitudMaxima(mascota.Nombre, 80, "Nombre")
            .Seleccionado(mascota.Especie, "Especie")
            .Requerido(mascota.Raza, "Raza")
            .LongitudMaxima(mascota.Raza, 80, "Raza")
            .Seleccionado(mascota.Sexo, "Sexo")
            .Regla(nacimiento <= hoy, "Fecha de nacimiento",
                $"{nacimiento:dd/MM/yyyy} es posterior a hoy ({hoy:dd/MM/yyyy}); no puede estar en el futuro.")
            .Regla(nacimiento >= hoy.AddYears(-40), "Fecha de nacimiento",
                $"{nacimiento:dd/MM/yyyy} indicaría más de 40 años de edad; revise el año.")
            .LongitudMaxima(mascota.Color, 80, "Color")
            .Regla(mascota.Peso > 0, "Peso (kg)", "debe ser mayor que 0.")
            .LongitudMaxima(mascota.Observaciones, 500, "Observaciones")
            .Resultado();
    }
}
