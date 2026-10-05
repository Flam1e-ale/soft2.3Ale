using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;
    public class ClienteServicio:IClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteServicio(IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio=clienteRepositorio;   
    }
        public List<Cliente> ObtenerTodos() => _clienteRepositorio.ObtenerTodos();

        public Cliente? ObtenerPorId(int id) => _clienteRepositorio.ObtenerPorId(id);

        public int Crear(Cliente cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));
        return _clienteRepositorio.Crear(cliente);
    }
    public void Actualizar(Cliente cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));
        _clienteRepositorio.Actualizar(cliente);
    }

    public void Eliminar(int id) => _clienteRepositorio.Eliminar(id);
    }
    
