using Persistencia.Entidades;

namespace Persistencia.Interfaces;
public interface IDireccionRepositorio
{
    List<Direccion> ObtenerTodos();
    Direccion? ObtenerPorId(int id);
    void Agregar(Direccion direccion);
    void Eliminar(int id);
}