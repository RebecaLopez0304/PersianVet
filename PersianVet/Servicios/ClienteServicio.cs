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

    public List<Cliente> Listar(string filtro, bool soloActivos = true)
    {
        return clienteDatos.Listar(filtro, soloActivos);
    }

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

    public void Desactivar(int idCliente)
    {
        clienteDatos.Desactivar(idCliente);
    }

    private static ResultadoOperacion Validar(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nombres) || string.IsNullOrWhiteSpace(cliente.Apellidos))
        {
            return ResultadoOperacion.Error("Escriba los nombres y apellidos del cliente.");
        }

        if (!Validaciones.EsTelefonoValido(cliente.Telefono))
        {
            return ResultadoOperacion.Error("Escriba un teléfono válido de al menos 8 números.");
        }

        if (!Validaciones.EsCorreoValido(cliente.Correo))
        {
            return ResultadoOperacion.Error("Escriba un correo electrónico válido.");
        }

        if (string.IsNullOrWhiteSpace(cliente.Direccion))
        {
            return ResultadoOperacion.Error("Escriba la dirección del cliente.");
        }

        return ResultadoOperacion.Correcto(string.Empty);
    }
}
