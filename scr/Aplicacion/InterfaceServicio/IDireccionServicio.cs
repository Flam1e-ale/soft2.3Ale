using Persistencia.Entidades;
namespace Aplicacion.InterfaceServicio;
 public interface IDireccionServicio
    {
        List<Direccion> ObtenerTodos();
        Direccion? ObtenerPorId(int id);
         int Crear(Direccion direccion);
         void Actualizar(Direccion direccion);
         void Eliminar(int id);
    }
