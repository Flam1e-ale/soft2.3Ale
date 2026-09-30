using Persistencia.Entidades;
namespace Aplicacion.Servicios;

    public interface IPaqueteServicio
    {
        List<Paquete> ObtenerTodos();
    }
    public class PaqueteServicio:IPaqueteServicio
    {
        private readonly IPaqueteServicio paqueteServicio;

    public PaqueteServicio(PaqueteServicio paqueteServicio)
    {
        this.paqueteServicio=paqueteServicio;
    }
    public List<Paquete> ObtenerTodos()
    {
        return paqueteServicio.ObtenerTodos().ToList();
    }
    }
