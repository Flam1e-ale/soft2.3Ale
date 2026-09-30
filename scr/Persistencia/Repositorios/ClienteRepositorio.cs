
using Persistencia.Entidades;
using Persistencia.Interfaces;
namespace Persistencia.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly List<Cliente> clientes = new();

    public List<Cliente> ObtenerTodos()
    {
        return clientes;
    }

    public Cliente? ObtenerPorId(int id)
    {
        return clientes.FirstOrDefault(c => c.Id == id);
    }

    public void Agregar(Cliente cliente)
    {
        clientes.Add(cliente);
    }

    public void Eliminar(int id)
    {
        Cliente? cliente = ObtenerPorId(id);

        if (cliente != null)
        {
            clientes.Remove(cliente);
        }
    }
}