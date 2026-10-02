using Persistencia.Entidades;

namespace Persistencia.Interfaces;
public interface IPaqueteRepositorio
{
    List<Paquete> ObtenerTodos();
    Paquete? ObtenerPorId(int id);
     int Crear(Paquete paquete);
    void Actualizar(Paquete paquete);
    void Eliminar(int id);
}