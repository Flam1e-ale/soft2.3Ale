using Persistencia.Entidades;

namespace Aplicacion.Repositorios;

public interface IClienteRepositorio
{
    List<Cliente> ObtenerTodos();
    Cliente? ObtenerPorId(int id);
    void Agregar(Cliente cliente);
    void Eliminar(int id);
}