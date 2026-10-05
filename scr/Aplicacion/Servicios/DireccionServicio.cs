using Persistencia.Entidades;
using Persistencia.Interfaces;
using Aplicacion.InterfaceServicio;
namespace Aplicacion.Servicios;

        public class DireccionServicio:IDireccionServicio
        {
            private readonly IDireccionRepositorio direccionRepositorio;


        public DireccionServicio(IDireccionRepositorio direccionRepositorio)

        {
            this.direccionRepositorio=direccionRepositorio;
        }
        public List<Direccion> ObtenerTodos()
        {
            return direccionRepositorio.ObtenerTodos();
        }
        }
