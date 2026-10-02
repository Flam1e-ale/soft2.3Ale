using Persistencia.Entidades;

namespace Persistencia.Interfaces;
public interface IDireccionRepositorio
{
    List<Direccion> ObtenerTodos();
    Direccion? ObtenerPorId(int id);
    int Crear(Direccion direccion);
    void Actualizar(Direccion direccion);
    void Eliminar(int id);
}