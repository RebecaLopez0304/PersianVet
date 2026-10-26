using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones de clientes.
/// </summary>
public class ClienteServicio
{
    private readonly IClienteDatos clienteDatos;

    public ClienteServicio(IClienteDatos clienteDatos)
    {
        this.clienteDatos = clienteDatos;
    }

    /// <inheritdoc cref="IClienteDatos.Listar(string, bool)"/>
    public List<Cliente> Listar(string filtro, bool soloActivos = true)
    {
        return clienteDatos.Listar(filtro, soloActivos);
    }

    /// <summary>
    /// Valida los nombres, el teléfono, el correo y la dirección, y registra el cliente.
    /// </summary>
    /// <param name="cliente">Datos del cliente.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Cliente cliente)
    {
        ResultadoOperacion validacion = Validar(cliente);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        cliente.IdCliente = clienteDatos.Guardar(cliente);
        return ResultadoOperacion.Correcto("El cliente se guardó correctamente.");
    }

    /// <summary>
    /// Valida los datos y modifica un cliente existente.
    /// </summary>
    /// <param name="cliente">Datos del cliente, con su identificador.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Actualizar(Cliente cliente)
    {
        if (cliente.IdCliente <= 0)
        {
            return ResultadoOperacion.Error("Seleccione un cliente de la lista.");
        }

        ResultadoOperacion validacion = Validar(cliente);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        clienteDatos.Actualizar(cliente);
        return ResultadoOperacion.Correcto("El cliente se modificó correctamente.");
    }

    /// <inheritdoc cref="IClienteDatos.Desactivar(int)"/>
    public void Desactivar(int idCliente)
    {
        clienteDatos.Desactivar(idCliente);
    }

    private ResultadoOperacion Validar(Cliente cliente)
    {
        Validador validador = new Validador()
            .Requerido(cliente.Nombres, "Nombres")
            .LongitudMaxima(cliente.Nombres, 80, "Nombres")
            .Requerido(cliente.Apellidos, "Apellidos")
            .LongitudMaxima(cliente.Apellidos, 80, "Apellidos")
            .Telefono(cliente.Telefono)
            .Correo(cliente.Correo)
            .Requerido(cliente.Direccion, "Dirección")
            .LongitudMaxima(cliente.Direccion, 200, "Dirección")
            .Regla(cliente.FechaRegistro.Date <= DateTime.Today, "Fecha de registro",
                $"{cliente.FechaRegistro:dd/MM/yyyy} está en el futuro; use la fecha de hoy o una anterior.");

        // El correo repetido se revisa solo si lo demás es válido, para no consultar la base sin necesidad.
        if (validador.EsValido)
        {
            string correo = cliente.Correo.Trim();
            string? otroCliente = clienteDatos.ObtenerNombreConCorreo(correo, cliente.IdCliente);
            validador.Regla(otroCliente is null, "Correo", $"\"{correo}\" ya está registrado para el cliente {otroCliente}.");
        }

        return validador.Resultado();
    }
}
