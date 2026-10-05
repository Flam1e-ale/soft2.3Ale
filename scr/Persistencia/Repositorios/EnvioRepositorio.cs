using Dapper;
using Persistencia.Conexion;
using Persistencia.Entidades;
using Persistencia.Interfaces;

namespace Persistencia.Repositorios;

public class EnvioRepositorio : IEnvioRepositorio
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EnvioRepositorio(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Envio? ObtenerPorId(int idEnvio)
    {
        using var connection = _connectionFactory.CrearConexion();

        string sql = @"
           SELECT
            e.IdEnvio,
            e.IdModalidad,
            e.Distancia,
            e.Costo,
            e.TiempoEstimado,
            e.FechaCreacion,
            e.Estado,

            c.IdCliente,
            c.Nombre,
            c.Email,

            p.IdPaquete,
            p.Peso,
            p.Alto,
            p.Ancho,
            p.Largo,

            do.IdDireccion AS IdDireccionOrigen,
            do.Calle AS CalleOrigen,
            do.Numero AS NumeroOrigen,
            do.Ciudad AS CiudadOrigen,

            dd.IdDireccion AS IdDireccionDestino,
            dd.Calle AS CalleDestino,
            dd.Numero AS NumeroDestino,
            dd.Ciudad AS CiudadDestino

        FROM Envio e

        INNER JOIN Cliente c
            ON e.IdCliente = c.IdCliente

        INNER JOIN Paquete p
            ON e.IdPaquete = p.IdPaquete

        INNER JOIN Direccion do
            ON e.IdDireccionOrigen = do.IdDireccion

        INNER JOIN Direccion dd
            ON e.IdDireccionDestino = dd.IdDireccion

        WHERE e.IdEnvio = @IdEnvio;
        ";

        var envio = connection.QueryFirstOrDefault<EnvioDatos>(
            sql,
            new { IdEnvio = idEnvio }
        );

        if (envio == null)
            return null;

        return CrearEnvio(envio);
    }

    public List<Envio> ObtenerTodos()
    {
        using var connection = _connectionFactory.CrearConexion();

        string sql = @"
            SELECT
                e.IdEnvio,
                e.IdCliente,
                e.IdPaquete,
                e.IdModalidad,
                e.IdDireccionOrigen,
                e.IdDireccionDestino,
                e.Distancia,
                e.Costo,
                e.TiempoEstimado,
                e.FechaCreacion,
                e.Estado
            FROM Envio e;
        ";

        var envios = connection.Query<EnvioDatos>(sql).ToList();

        return envios
            .Select(CrearEnvio)
            .ToList();
    }

    private Envio CrearEnvio(EnvioDatos datos)
    {
        throw new NotImplementedException();
    }
}