using Persistencia.Entidades;

namespace Aplicacion.Repositorios;

public interface IPaqueteRepositorio
{
    List<Paquete> ObtenerTodos();
    Paquete? ObtenerPorId(int id);
    void Agregar(Paquete paquete);
    void Eliminar(int id);
}