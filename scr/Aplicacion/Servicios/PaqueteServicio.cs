using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;


    public class PaqueteServicio:IPaqueteServicio
    {
        private readonly IPaqueteRepositorio paqueteRepositorio;

    public PaqueteServicio(IPaqueteRepositorio paqueteRepositorio)
    {
        this.paqueteRepositorio=paqueteRepositorio;
    }
    public List<Paquete> ObtenerTodos()
    {
        return paqueteRepositorio.ObtenerTodos();
    }
    }
