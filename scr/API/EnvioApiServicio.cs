using Aplicacion.InterfaceServicio;
using Persistencia.Entidades;
using Persistencia.Interfaces;

namespace API;

public class EnvioApiServicio : IEnvioApiServicio
{
    private readonly IEnvioServicio _envioServicio;
    private readonly IClienteServicio _clienteServicio;
    private readonly IDireccionServicio _direccionServicio;
    private readonly IEnvioRepositorio _envioRepositorio;

    public EnvioApiServicio(
        IEnvioServicio envioServicio,
        IClienteServicio clienteServicio,
        IDireccionServicio direccionServicio,
        IEnvioRepositorio envioRepositorio)
    {
        _envioServicio = envioServicio;
        _clienteServicio = clienteServicio;
        _direccionServicio = direccionServicio;
        _envioRepositorio = envioRepositorio;
    }

    public EnvioResponseDto Crear(EnvioRequestDto solicitud)
    {
        ValidarSolicitud(solicitud);

        int idCliente = _clienteServicio.Crear(solicitud.Cliente);
        int idOrigen = _direccionServicio.Crear(solicitud.Origen);
        int idDestino = _direccionServicio.Crear(solicitud.Destino);

        solicitud.Cliente.IdCliente = idCliente;
        solicitud.Origen.IdDireccion = idOrigen;
        solicitud.Destino.IdDireccion = idDestino;

        var envio = _envioServicio.RegistrarEnvio(
            idCliente,
            idOrigen,
            idDestino,
            solicitud.Paquete.Peso,
            solicitud.Paquete.Alto,
            solicitud.Paquete.Ancho,
            solicitud.Paquete.Largo,
            solicitud.Distancia,
            solicitud.IdModalidad);

        return Convertir(envio);
    }

    public IReadOnlyList<EnvioResponseDto> Listar()
    {
        return _envioServicio.ListarTodos()
            .Select(Convertir)
            .ToList();
    }

    public EnvioResponseDto? Obtener(int id)
    {
        var envio = _envioServicio.ObtenerPorId(id);

        return envio == null ? null : Convertir(envio);
    }

    public CotizacionDto? Cotizar(int id)
    {
        var envio = _envioServicio.ObtenerPorId(id);

        if (envio == null)
            return null;

        return new CotizacionDto
        {
            Id = envio.Id,
            Costo = envio.CalcularCosto(),
            DiasEstimados = envio.CalcularTiempoEntrega()
        };
    }

    public EnvioResponseDto? Actualizar(
        int id,
        EnvioRequestDto solicitud)
    {
        var existente = _envioServicio.ObtenerPorId(id);

        if (existente == null)
            return null;

        ValidarSolicitud(solicitud);

        int idCliente = _clienteServicio.Crear(solicitud.Cliente);
        int idOrigen = _direccionServicio.Crear(solicitud.Origen);
        int idDestino = _direccionServicio.Crear(solicitud.Destino);

        var nuevoEnvio = ConstruirEnvio(
            id,
            solicitud,
            idCliente,
            idOrigen,
            idDestino);

        double costo = nuevoEnvio.CalcularCosto();
        int tiempo = nuevoEnvio.CalcularTiempoEntrega();

        bool actualizado = _envioRepositorio.ActualizarEnvioCompleto(
            id,
            idCliente,
            idOrigen,
            idDestino,
            solicitud.Paquete.Peso,
            solicitud.Paquete.Alto,
            solicitud.Paquete.Ancho,
            solicitud.Paquete.Largo,
            solicitud.Distancia,
            solicitud.IdModalidad,
            costo,
            tiempo);

        if (!actualizado)
            return null;

        var resultado = _envioServicio.ObtenerPorId(id);

        return resultado == null ? null : Convertir(resultado);
    }

    public bool Eliminar(int id)
    {
        return _envioRepositorio.Eliminar(id);
    }

    public bool CambiarEstado(int id, string nuevoEstado)
    {
        if (_envioServicio.ObtenerPorId(id) == null)
            return false;

        _envioServicio.CambiarEstado(id, nuevoEstado);

        return true;
    }

    private Envio ConstruirEnvio(
        int id,
        EnvioRequestDto solicitud,
        int idCliente,
        int idOrigen,
        int idDestino)
    {
        solicitud.Cliente.IdCliente = idCliente;
        solicitud.Origen.IdDireccion = idOrigen;
        solicitud.Destino.IdDireccion = idDestino;

        return solicitud.IdModalidad switch
        {
            EnvioEstandar.IdModalidad =>
                new EnvioEstandar(
                    id, solicitud.Cliente, solicitud.Paquete,
                    solicitud.Origen, solicitud.Destino,
                    solicitud.Distancia),

            EnvioExpress.IdModalidad =>
                new EnvioExpress(
                    id, solicitud.Cliente, solicitud.Paquete,
                    solicitud.Origen, solicitud.Destino,
                    solicitud.Distancia),

            EnvioPrioritario.IdModalidad =>
                new EnvioPrioritario(
                    id, solicitud.Cliente, solicitud.Paquete,
                    solicitud.Origen, solicitud.Destino,
                    solicitud.Distancia),

            _ => throw new ArgumentException("Modalidad inválida.")
        };
    }

    private void ValidarSolicitud(EnvioRequestDto solicitud)
    {
        if (solicitud == null)
            throw new ArgumentException("La solicitud no puede estar vacía.");

        if (solicitud.Cliente == null ||
            string.IsNullOrWhiteSpace(solicitud.Cliente.Nombre) ||
            string.IsNullOrWhiteSpace(solicitud.Cliente.Email))
            throw new ArgumentException("Los datos del cliente son obligatorios.");

        if (solicitud.Origen == null ||
            string.IsNullOrWhiteSpace(solicitud.Origen.Calle) ||
            string.IsNullOrWhiteSpace(solicitud.Origen.Ciudad))
            throw new ArgumentException("La dirección de origen es inválida.");

        if (solicitud.Destino == null ||
            string.IsNullOrWhiteSpace(solicitud.Destino.Calle) ||
            string.IsNullOrWhiteSpace(solicitud.Destino.Ciudad))
            throw new ArgumentException("La dirección de destino es inválida.");

        if (solicitud.Paquete == null)
            throw new ArgumentException("El paquete es obligatorio.");

        if (solicitud.IdModalidad < 1 || solicitud.IdModalidad > 3)
            throw new ArgumentException("La modalidad es inválida.");

        if (solicitud.Distancia <= 0)
            throw new ArgumentException("La distancia debe ser mayor a cero.");
    }

    private EnvioResponseDto Convertir(Envio envio)
    {
        int idModalidad = envio switch
        {
            EnvioEstandar => EnvioEstandar.IdModalidad,
            EnvioExpress => EnvioExpress.IdModalidad,
            EnvioPrioritario => EnvioPrioritario.IdModalidad,
            _ => 0
        };

        return new EnvioResponseDto
        {
            Id = envio.Id,
            IdModalidad = idModalidad,
            Cliente = envio.Cliente,
            Paquete = envio.Paquete,
            Origen = envio.Origen,
            Destino = envio.Destino,
            Distancia = envio.Distancia,
            Estado = envio.Estado,
            Costo = envio.CalcularCosto(),
            DiasEstimados = envio.CalcularTiempoEntrega()
        };
    }
}