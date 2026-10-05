using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;
    public class ClienteServicio:IClienteServicio
    {
        private readonly IClienteRepositorio clienteRepositorio;

        public ClienteServicio(IClienteRepositorio clienteRepositorio)
    {
        this.clienteRepositorio=clienteRepositorio;   
    }
        public List<Cliente> ObtenerTodos()
    {
         return clienteRepositorio.ObtenerTodos();
    } 
    
}