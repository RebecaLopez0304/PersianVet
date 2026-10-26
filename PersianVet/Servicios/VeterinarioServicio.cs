using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones de veterinarios.
/// </summary>
public class VeterinarioServicio
{
    private readonly IVeterinarioDatos veterinarioDatos;

    public VeterinarioServicio(IVeterinarioDatos veterinarioDatos)
    {
        this.veterinarioDatos = veterinarioDatos;
    }

    /// <inheritdoc cref="IVeterinarioDatos.Listar(string, bool)"/>
    public List<Veterinario> Listar(string filtro, bool soloActivos = true)
    {
        return veterinarioDatos.Listar(filtro, soloActivos);
    }

    /// <summary>
    /// Valida los nombres, la especialidad y el teléfono, y registra el veterinario.
    /// </summary>
    /// <param name="veterinario">Datos del veterinario.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Veterinario veterinario)
    {
        ResultadoOperacion validacion = Validar(veterinario);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        veterinario.IdVeterinario = veterinarioDatos.Guardar(veterinario);
        return ResultadoOperacion.Correcto("El veterinario se guardó correctamente.");
    }

    /// <summary>
    /// Valida los datos y modifica un veterinario existente.
    /// </summary>
    /// <param name="veterinario">Datos del veterinario, con su identificador.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Actualizar(Veterinario veterinario)
    {
        if (veterinario.IdVeterinario <= 0)
        {
            return ResultadoOperacion.Error("Seleccione un veterinario de la lista.");
        }

        ResultadoOperacion validacion = Validar(veterinario);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        veterinarioDatos.Actualizar(veterinario);
        return ResultadoOperacion.Correcto("El veterinario se modificó correctamente.");
    }

    /// <inheritdoc cref="IVeterinarioDatos.Desactivar(int)"/>
    public void Desactivar(int idVeterinario)
    {
        veterinarioDatos.Desactivar(idVeterinario);
    }

    private static ResultadoOperacion Validar(Veterinario veterinario)
    {
        return new Validador()
            .Requerido(veterinario.Nombres, "Nombres")
            .LongitudMaxima(veterinario.Nombres, 80, "Nombres")
            .Requerido(veterinario.Apellidos, "Apellidos")
            .LongitudMaxima(veterinario.Apellidos, 80, "Apellidos")
            .Requerido(veterinario.Especialidad, "Especialidad")
            .LongitudMaxima(veterinario.Especialidad, 100, "Especialidad")
            .Telefono(veterinario.Telefono)
            .Resultado();
    }
}
