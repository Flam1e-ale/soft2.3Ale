using Persistencia.Entidades;

namespace Persistencia.Interfaces;

public interface IClienteRepositorio
{
    List<Cliente> ObtenerTodos();
    Cliente? ObtenerPorId(int id);
    void Agregar(Cliente cliente);
    void Eliminar(int id);
}