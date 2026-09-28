using Persistencia.Entidades;

namespace Aplicacion.Repositorios;

public class PaqueteRepositorio : IPaqueteRepositorio
{
    private readonly List<Paquete> paquetes = new();

    public List<Paquete> ObtenerTodos()
    {
        return paquetes;
    }

    public Paquete? ObtenerPorId(int id)
    {
        return paquetes.FirstOrDefault(p => p.Id == id);
    }

    public void Agregar(Paquete paquete)
    {
        paquetes.Add(paquete);
    }

    public void Eliminar(int id)
    {
        Paquete? paquete = ObtenerPorId(id);

        if (paquete!= null)
        {
            paquetes.Remove(paquete);
        }
    }
}