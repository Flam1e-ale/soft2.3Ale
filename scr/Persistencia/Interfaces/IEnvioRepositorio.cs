using Persistencia.Entidades;

namespace Persistencia.Interfaces;

public interface IEnvioRepositorio
{
    Envio? ObtenerPorId(int idEnvio);
    List<Envio> ObtenerTodos();

    // Operaciones con Stored Procedures
    int AltaEnvioCompleto(
        int idCliente,
        int idOrigen,
        int idDestino,
        double peso,
        double alto,
        double ancho,
        double largo,
        double distancia,
        int idModalidad,
        double costo,
        int tiempoEstimado);

    void CambiarEstado(int idEnvio, string nuevoEstado);
    void CancelarEnvio(int idEnvio);

    // Estadísticas
    IEnumerable<dynamic> ObtenerResumenEnvios(DateTime fechaInicio, DateTime fechaFin);
    IEnumerable<dynamic> ObtenerCostosPorModalidad(DateTime fechaInicio, DateTime fechaFin);
    IEnumerable<dynamic> ObtenerEstadosPorPeriodo(DateTime fechaInicio, DateTime fechaFin);
    IEnumerable<dynamic> ObtenerTiempoPromedio(DateTime fechaInicio, DateTime fechaFin);
    IEnumerable<dynamic> ObtenerFacturacionPorModalidad(DateTime fechaInicio, DateTime fechaFin);
}