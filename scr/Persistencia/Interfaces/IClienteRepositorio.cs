using Persistencia.Entidades;

namespace Persistencia.Interfaces;

public interface IClienteRepositorio
{
    List<Cliente> ObtenerTodos();
    Cliente? ObtenerPorId(int id);
    int CrearCliente(Cliente cliente);
    void Actualizar(Cliente cliente);
    void Eliminar(int id);
}