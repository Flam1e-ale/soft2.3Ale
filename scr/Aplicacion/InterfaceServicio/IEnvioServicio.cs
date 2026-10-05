using Persistencia.Entidades;

namespace Aplicacion.InterfaceServicio;

public interface IEnvioServicio
{
    Envio RegistrarEnvio(
        int idCliente,
        int idOrigen,
        int idDestino,
        double peso,
        double alto,
        double ancho,
        double largo,
        double distancia,
        int idModalidad);

    void CambiarEstado(int idEnvio, string nuevoEstado);
    void CancelarEnvio(int idEnvio);
    Envio? ObtenerPorId(int idEnvio);
    List<Envio> ListarTodos();

    IEnumerable<dynamic> ConsultarResumen(DateTime desde, DateTime hasta);
    IEnumerable<dynamic> ConsultarCostos(DateTime desde, DateTime hasta);
    IEnumerable<dynamic> ConsultarEstados(DateTime desde, DateTime hasta);
    IEnumerable<dynamic> ConsultarTiempoPromedio(DateTime desde, DateTime hasta);
    IEnumerable<dynamic> ConsultarFacturacion(DateTime desde, DateTime hasta);
}