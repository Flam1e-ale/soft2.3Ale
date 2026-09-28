namespace Aplicacion;

    public interface IClienteServicio
    {
        List<Cliente> ObtenerTodos();
    }
    public class ClienteServicio:IClienteServicio
    {
        private readonly IClienteServicio clienteServicio;

        public ClienteServicio(IClienteServicio servicioCliente)
    {
        this.clienteServicio=servicioCliente;   
    }
        public List<Cliente> ObtenerTodos()
    {
         return clienteServicio.ObtenerTodos().ToList();
    } 
}