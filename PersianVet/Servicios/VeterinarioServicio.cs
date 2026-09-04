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

    public List<Veterinario> Listar(string filtro, bool soloActivos = true)
    {
        return veterinarioDatos.Listar(filtro, soloActivos);
    }

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

    public void Desactivar(int idVeterinario)
    {
        veterinarioDatos.Desactivar(idVeterinario);
    }

    private static ResultadoOperacion Validar(Veterinario veterinario)
    {
        if (string.IsNullOrWhiteSpace(veterinario.Nombres) || string.IsNullOrWhiteSpace(veterinario.Apellidos))
        {
            return ResultadoOperacion.Error("Escriba los nombres y apellidos del veterinario.");
        }

        if (string.IsNullOrWhiteSpace(veterinario.Especialidad))
        {
            return ResultadoOperacion.Error("Escriba la especialidad del veterinario.");
        }

        if (!Validaciones.EsTelefonoValido(veterinario.Telefono))
        {
            return ResultadoOperacion.Error("Escriba un teléfono válido de al menos 8 números.");
        }

        return ResultadoOperacion.Correcto(string.Empty);
    }
}
