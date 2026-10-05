using Persistencia.Entidades;
namespace Aplicacion.InterfaceServicio;
    public interface IPaqueteServicio
    {
        List<Paquete> ObtenerTodos();
        Paquete? ObtenerPorId(int id);
        int Crear(Paquete paquete);
        void Actualizar(Paquete paquete);
        void Eliminar(int id);
    }