using PersianVet.Modelos;

namespace PersianVet.Interfaces;

/// <summary>
/// Define las operaciones disponibles para clientes.
/// </summary>
public interface IClienteDatos
{
    List<Cliente> Listar(string filtro, bool soloActivos = true);
    int Guardar(Cliente cliente);
    void Actualizar(Cliente cliente);
    void Desactivar(int idCliente);
}
