using Dapper;
using Persistencia.Conexion;
using Persistencia.Entidades;
using Persistencia.Interfaces;
using System.Data;

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
            INNER JOIN Cliente c ON e.IdCliente = c.IdCliente
            INNER JOIN Paquete p ON e.IdPaquete = p.IdPaquete
            INNER JOIN Direccion do ON e.IdDireccionOrigen = do.IdDireccion
            INNER JOIN Direccion dd ON e.IdDireccionDestino = dd.IdDireccion
            WHERE e.IdEnvio = @IdEnvio;
        ";

        var datos = connection.QueryFirstOrDefault<EnvioDatos>(
            sql,
            new { IdEnvio = idEnvio }
        );

        return datos == null ? null : CrearEnvio(datos);
    }

    public List<Envio> ObtenerTodos()
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
            INNER JOIN Cliente c ON e.IdCliente = c.IdCliente
            INNER JOIN Paquete p ON e.IdPaquete = p.IdPaquete
            INNER JOIN Direccion do ON e.IdDireccionOrigen = do.IdDireccion
            INNER JOIN Direccion dd ON e.IdDireccionDestino = dd.IdDireccion;
        ";

        var lista = connection.Query<EnvioDatos>(sql).ToList();

        return lista.Select(CrearEnvio).ToList();
    }

    // ========== FACTORY POLIMÓRFICO ==========

    private Envio CrearEnvio(EnvioDatos d)
    {
        var cliente = new Cliente(d.Nombre, d.Email)
        {
            IdCliente = d.IdCliente
        };

        var paquete = new Paquete(
            d.Peso,
            d.Alto,
            d.Ancho,
            d.Largo)
        {
            IdPaquete = d.IdPaquete
        };

        var origen = new Direccion(
            d.CalleOrigen,
            d.NumeroOrigen,
            d.CiudadOrigen)
        {
            IdDireccion = d.IdDireccionOrigen
        };

        var destino = new Direccion(
            d.CalleDestino,
            d.NumeroDestino,
            d.CiudadDestino)
        {
            IdDireccion = d.IdDireccionDestino
        };

        Envio envio = d.IdModalidad switch
        {
            EnvioEstandar.IdModalidad =>
                new EnvioEstandar(
                    d.IdEnvio,
                    cliente,
                    paquete,
                    origen,
                    destino,
                    d.Distancia),

            EnvioExpress.IdModalidad =>
                new EnvioExpress(
                    d.IdEnvio,
                    cliente,
                    paquete,
                    origen,
                    destino,
                    d.Distancia),

            EnvioPrioritario.IdModalidad =>
                new EnvioPrioritario(
                    d.IdEnvio,
                    cliente,
                    paquete,
                    origen,
                    destino,
                    d.Distancia),

            _ => throw new InvalidOperationException(
                $"Modalidad desconocida: {d.IdModalidad}")
        };

        // Asignamos el estado persistido en la base de datos.
        envio.Estado = d.Estado;

        return envio;
    }

    public int AltaEnvioCompleto(
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
        int tiempoEstimado)
    {
        using var connection = _connectionFactory.CrearConexionAdmin();

        var parametros = new DynamicParameters();

        parametros.Add("p_idCliente", idCliente);
        parametros.Add("p_idOrigen", idOrigen);
        parametros.Add("p_idDestino", idDestino);
        parametros.Add("p_peso", peso);
        parametros.Add("p_alto", alto);
        parametros.Add("p_ancho", ancho);
        parametros.Add("p_largo", largo);
        parametros.Add("p_distancia", distancia);
        parametros.Add("p_idModalidad", idModalidad);
        parametros.Add("p_costo", costo);
        parametros.Add("p_tiempoEstimado", tiempoEstimado);

        parametros.Add(
            "p_idEnvio",
            dbType: DbType.Int32,
            direction: ParameterDirection.Output
        );

        connection.Execute(
            "altaEnvioCompleto",
            parametros,
            commandType: CommandType.StoredProcedure
        );

        return parametros.Get<int>("p_idEnvio");
    }

    public void CambiarEstado(int idEnvio, string nuevoEstado)
    {
        using var connection = _connectionFactory.CrearConexion();

        connection.Execute(
            "cambiarEstadoEnvio",
            new
            {
                p_idEnvio = idEnvio,
                p_nuevoEstado = nuevoEstado
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public void CancelarEnvio(int idEnvio)
    {
        using var connection = _connectionFactory.CrearConexion();

        connection.Execute(
            "cancelarEnvio",
            new { p_idEnvio = idEnvio },
            commandType: CommandType.StoredProcedure
        );
    }

   
public bool ActualizarEnvioCompleto(
    int idEnvio,
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
    int tiempoEstimado)
{
    using var connection = _connectionFactory.CrearConexionAdmin();

    // CORRECCIÓN: abrir la conexión antes de iniciar la transacción.
    connection.Open();

    using var transaction = connection.BeginTransaction();

    try
    {
        var idPaquete = connection.QuerySingleOrDefault<int?>(
            @"SELECT IdPaquete
              FROM Envio
              WHERE IdEnvio = @idEnvio
              FOR UPDATE",
            new { idEnvio },
            transaction
        );

        if (idPaquete == null)
            return false;

        connection.Execute(@"
            UPDATE Paquete
            SET Peso = @peso,
                Alto = @alto,
                Ancho = @ancho,
                Largo = @largo
            WHERE IdPaquete = @idPaquete",
            new
            {
                peso,
                alto,
                ancho,
                largo,
                idPaquete
            },
            transaction
        );

        connection.Execute(@"
            UPDATE Envio
            SET IdCliente = @idCliente,
                IdDireccionOrigen = @idOrigen,
                IdDireccionDestino = @idDestino,
                IdModalidad = @idModalidad,
                Distancia = @distancia,
                Costo = @costo,
                TiempoEstimado = @tiempoEstimado
            WHERE IdEnvio = @idEnvio",
            new
            {
                idCliente,
                idOrigen,
                idDestino,
                idModalidad,
                distancia,
                costo,
                tiempoEstimado,
                idEnvio
            },
            transaction
        );

        transaction.Commit();

        return true;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}



public bool Eliminar(int idEnvio)
{
    using var connection = _connectionFactory.CrearConexionAdmin();

    // Abrimos la conexión antes de iniciar la transacción.
    connection.Open();

    using var transaction = connection.BeginTransaction();

    try
    {
        var idPaquete = connection.QuerySingleOrDefault<int?>(
            @"SELECT IdPaquete
              FROM Envio
              WHERE IdEnvio = @idEnvio
              FOR UPDATE",
            new { idEnvio },
            transaction
        );

        if (idPaquete == null)
        {
            transaction.Rollback();
            return false;
        }

        // Eliminamos primero el historial.
        connection.Execute(
            "DELETE FROM HistorialEstado WHERE IdEnvio = @idEnvio",
            new { idEnvio },
            transaction
        );

        // Eliminamos el envío.
        connection.Execute(
            "DELETE FROM Envio WHERE IdEnvio = @idEnvio",
            new { idEnvio },
            transaction
        );

        // Eliminamos el paquete solamente si no está asociado
        // a ningún otro envío.
        connection.Execute(@"
            DELETE FROM Paquete
            WHERE IdPaquete = @idPaquete
            AND NOT EXISTS (
                SELECT 1
                FROM Envio
                WHERE IdPaquete = @idPaquete
            )",
            new { idPaquete },
            transaction
        );

        transaction.Commit();

        return true;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}


    // ========== ESTADÍSTICAS ==========

    public IEnumerable<dynamic> ObtenerResumenEnvios(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = _connectionFactory.CrearConexion();

        return connection.Query(
            "obtenerResumenEnvios",
            new
            {
                p_fechaInicio = fechaInicio,
                p_fechaFin = fechaFin
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public IEnumerable<dynamic> ObtenerCostosPorModalidad(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = _connectionFactory.CrearConexion();

        return connection.Query(
            "obtenerCostosPorModalidad",
            new
            {
                p_fechaInicio = fechaInicio,
                p_fechaFin = fechaFin
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public IEnumerable<dynamic> ObtenerEstadosPorPeriodo(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = _connectionFactory.CrearConexion();

        return connection.Query(
            "obtenerEstadosPorPeriodo",
            new
            {
                p_fechaInicio = fechaInicio,
                p_fechaFin = fechaFin
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public IEnumerable<dynamic> ObtenerTiempoPromedio(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = _connectionFactory.CrearConexion();

        return connection.Query(
            "obtenerTiempoPromedio",
            new
            {
                p_fechaInicio = fechaInicio,
                p_fechaFin = fechaFin
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public IEnumerable<dynamic> ObtenerFacturacionPorModalidad(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        using var connection = _connectionFactory.CrearConexion();

        return connection.Query(
            "obtenerFacturacionPorModalidad",
            new
            {
                p_fechaInicio = fechaInicio,
                p_fechaFin = fechaFin
            },
            commandType: CommandType.StoredProcedure
        );
    }
}
