using Perisitencia.Entidades;

namespace Aplicacion.Repositorios;

public interface IDireccionRepositorio
{
    List<Direccion> ObtenerTodos();
    Direccion? ObtenerPorId(int id);
    void Agregar(Direccion direccion);
    void Eliminar(int id);
}