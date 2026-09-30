using Persistencia.Entidades;

namespace Persistencia.Interfaces;
public interface IPaqueteRepositorio
{
    List<Paquete> ObtenerTodos();
    Paquete? ObtenerPorId(int id);
    void Agregar(Paquete paquete);
    void Eliminar(int id);
}