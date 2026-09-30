using Persistencia.Entidades;
using Persistencia.Interfaces;
namespace Persistencia.Repositorios;

public class DireccionRepositorio : IDireccionRepositorio
{
    private readonly List<Direccion> direcciones = new();

    public List<Direccion> ObtenerTodos()
    {
        return direcciones;
    }

    public Direccion? ObtenerPorId(int id)
    {
        return direcciones.FirstOrDefault(d => d.Id == id);
    }

    public void Agregar(Direccion direccion)
    {
        direcciones.Add(direccion);
    }

    public void Eliminar(int id)
    {
        Direccion? direccion = ObtenerPorId(id);

        if (direccion != null)
        {
            direcciones.Remove(direccion);
        }
    }
}