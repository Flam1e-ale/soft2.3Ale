using Aplicacion.InterfaceServicio;
using Persistencia.Entidades;
using Persistencia.Interfaces;

namespace Aplicacion.Servicios;

public class EnvioServicio : IEnvioServicio
{
    private readonly IEnvioRepositorio _envioRepositorio;

    public EnvioServicio(IEnvioRepositorio envioRepositorio)
    {
        _envioRepositorio = envioRepositorio;
    }

    public Envio RegistrarEnvio(
        int idCliente,
        int idOrigen,
        int idDestino,
        double peso,
        double alto,
        double ancho,
        double largo,
        double distancia,
        int idModalidad)
    {
        // Validaciones de negocio
        if (peso <= 0 || peso > 50)
            throw new ArgumentException("Peso inválido");
             if (alto <= 0 || alto > 20)
            throw new ArgumentException("Alto inválido");
        if (ancho <= 0 || ancho > 20)
            throw new ArgumentException("Ancho inválido");
        if (largo <= 0 || largo > 20)
            throw new ArgumentException("Largo inválido");
        if (distancia <= 0)
            throw new ArgumentException("Distancia debe ser mayor a cero");
        if (idModalidad < 1 || idModalidad > 3)
            throw new ArgumentException("Modalidad inválida");

        
        var paqueteTemp = new Paquete(peso, alto, ancho, largo);
        var clienteTemp = new Cliente("temp", "temp@temp.com");
        var origenTemp = new Direccion("temp", 1, "temp");
        var destinoTemp = new Direccion("temp", 1, "temp");

        
        Envio envio = idModalidad switch
        {
            EnvioEstandar.IdModalidad => new EnvioEstandar(0, clienteTemp, paqueteTemp, origenTemp, destinoTemp, distancia),
            EnvioExpress.IdModalidad => new EnvioExpress(0, clienteTemp, paqueteTemp, origenTemp, destinoTemp, distancia),
            EnvioPrioritario.IdModalidad => new EnvioPrioritario(0, clienteTemp, paqueteTemp, origenTemp, destinoTemp, distancia),
            _ => throw new ArgumentException("Modalidad no soportada")
        };

        double costo = envio.CalcularCosto();
        int tiempo = envio.CalcularTiempoEntrega();
           

        // Persistimos con el SP
        _envioRepositorio.AltaEnvioCompleto(
            idCliente, idOrigen, idDestino,
            peso, alto, ancho, largo,
            distancia, idModalidad, costo, tiempo);

        return envio;
    }

    public void CambiarEstado(int idEnvio, string nuevoEstado)
    {
        if (string.IsNullOrWhiteSpace(nuevoEstado))
            throw new ArgumentException("Estado inválido");

        string estado = nuevoEstado.ToUpper().Trim();
        string[] estadosValidos = { "PENDIENTE", "EN_PROCESO", "ENTREGADO", "CANCELADO" };

        if (!estadosValidos.Contains(estado))
            throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}");

        _envioRepositorio.CambiarEstado(idEnvio, estado);
    }

    public void CancelarEnvio(int idEnvio)
    {
        _envioRepositorio.CancelarEnvio(idEnvio);
    }

    public Envio? ObtenerPorId(int idEnvio) => _envioRepositorio.ObtenerPorId(idEnvio);

    public List<Envio> ListarTodos() => _envioRepositorio.ObtenerTodos();

    public IEnumerable<dynamic> ConsultarResumen(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return _envioRepositorio.ObtenerResumenEnvios(desde, hasta);
    }

    public IEnumerable<dynamic> ConsultarCostos(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return _envioRepositorio.ObtenerCostosPorModalidad(desde, hasta);
    }

    public IEnumerable<dynamic> ConsultarEstados(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return _envioRepositorio.ObtenerEstadosPorPeriodo(desde, hasta);
    }

    public IEnumerable<dynamic> ConsultarTiempoPromedio(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return _envioRepositorio.ObtenerTiempoPromedio(desde, hasta);
    }

    public IEnumerable<dynamic> ConsultarFacturacion(DateTime desde, DateTime hasta)
    {
        ValidarFechas(desde, hasta);
        return _envioRepositorio.ObtenerFacturacionPorModalidad(desde, hasta);
    }

    private static void ValidarFechas(DateTime desde, DateTime hasta)
    {
        if (desde > hasta)
            throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha de fin");
    }
}