using Persistencia.Entidades;
namespace Aplicacion.InterfaceServicio;
 public interface IClienteServicio
    {
        List<Cliente> ObtenerTodos();
        Cliente? ObtenerPorId(int id);
        int Crear(Cliente cliente);
        void Actualizar(Cliente cliente);
        void Eliminar(int id);
    }