namespace Aplicacion;

    public interface IDireccionServicio
    {
        List<Direccion> ObtenerTodos();
    }

    public class DireccionServicio:IDireccionServicio
    {
        private readonly IDireccionServicio direccionServicio;


    public DireccionServicio(DireccionServicio direccionServicio)

    {
        this.direccionServicio=direccionServicio;
    }
    public List<Direccion> ObtenerTodos()
    {
        return direccionServicio.ObtenerTodos().ToList();
    }
    }
