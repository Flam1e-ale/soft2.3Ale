using Persistencia.Entidades;
using Persistencia.Interfaces;
using Persistencia.Conexion;
using MySqlConnector;
using Dapper;
namespace Persistencia.Repositorios;

public class PaqueteRepositorio : IPaqueteRepositorio
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PaqueteRepositorio(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

     public Paquete? ObtenerPorId(int idPaquete)
        {
            using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                SELECT *
                FROM Paquete
                WHERE idPaquete = @idPaquete";

            return connection.QueryFirstOrDefault<Paquete>(
                sql,
                new { idPaquete });
        }

        public List<Paquete> ObtenerTodos()
        {
            using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                SELECT *
                FROM Paquete";

            return connection.Query<Paquete>(sql).ToList();
        }

        public int Crear(Paquete paquete)
        {
            using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                INSERT INTO Paquete
                (peso, alto, ancho, largo)
                VALUES
                (@Peso, @Alto, @Ancho, @Largo);

                SELECT LAST_INSERT_ID();";

            return connection.QuerySingle<int>(sql, paquete);
        }

        public void Actualizar(Paquete paquete)
        {
            using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                UPDATE Paquete
                SET peso = @Peso,
                    alto = @Alto,
                    ancho = @Ancho,
                    largo = @Largo
                WHERE idPaquete = @IdPaquete";

            connection.Execute(sql, paquete);
        }

        public void Eliminar(int idPaquete)
        {
            using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                DELETE FROM Paquete
                WHERE idPaquete = @idPaquete";

            connection.Execute(sql, new { idPaquete });
        }

}