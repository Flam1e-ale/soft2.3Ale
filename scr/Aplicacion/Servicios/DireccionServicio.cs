using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;

 public class DireccionServicio:IDireccionServicio
{
     private readonly IDireccionRepositorio _direccionRepositorio;

    public DireccionServicio(IDireccionRepositorio direccionRepositorio)
    {
        _direccionRepositorio = direccionRepositorio;
    }        
    public List<Direccion> ObtenerTodos() => _direccionRepositorio.ObtenerTodos();

    public Direccion? ObtenerPorId(int id) => _direccionRepositorio.ObtenerPorId(id);

    public int Crear(Direccion direccion)
    {
        if (direccion == null)
            throw new ArgumentNullException(nameof(direccion));
        return _direccionRepositorio.Crear(direccion);
    }

    public void Actualizar(Direccion direccion)
    {
        if (direccion == null)
            throw new ArgumentNullException(nameof(direccion));
        _direccionRepositorio.Actualizar(direccion);
    }

    public void Eliminar(int id) => _direccionRepositorio.Eliminar(id);
}
