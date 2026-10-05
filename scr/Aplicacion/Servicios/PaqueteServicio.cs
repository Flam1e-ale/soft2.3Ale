using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;


public class PaqueteServicio:IPaqueteServicio
{
    private readonly IPaqueteRepositorio _paqueteRepositorio;

    public PaqueteServicio(IPaqueteRepositorio paqueteRepositorio)
    {
        _paqueteRepositorio = paqueteRepositorio;
    }
    
    public List<Paquete> ObtenerTodos() => _paqueteRepositorio.ObtenerTodos();

    public Paquete? ObtenerPorId(int id) => _paqueteRepositorio.ObtenerPorId(id);
    
    public int Crear(Paquete paquete)
    {
        if (paquete == null)
            throw new ArgumentNullException(nameof(paquete));
        return _paqueteRepositorio.Crear(paquete);
    }

    public void Actualizar(Paquete paquete)
    {
        if (paquete == null)
            throw new ArgumentNullException(nameof(paquete));
        _paqueteRepositorio.Actualizar(paquete);
    }

    public void Eliminar(int id) => _paqueteRepositorio.Eliminar(id);
}
