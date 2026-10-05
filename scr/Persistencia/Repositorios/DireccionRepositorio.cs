using Persistencia.Entidades;
using Persistencia.Interfaces;
using MySqlConnector;
using Dapper;
namespace Persistencia.Repositorios;

public class DireccionRepositorio : IDireccionRepositorio
{
     private readonly string _connectionString;

        public DireccionRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Direccion? ObtenerPorId(int idDireccion)
    {
        using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT *
                FROM Direccion
                WHERE idDireccion = @idDireccion";

            return connection.QueryFirstOrDefault<Direccion>(
                sql,
                new { idDireccion });

    }
      public List<Direccion> ObtenerTodos()
        {
            using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT *
                FROM Direccion";

            return connection.Query<Direccion>(sql).ToList();
        }
         public int Crear(Direccion direccion)
        {
            using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                INSERT INTO Direccion
                (calle, ciudad, numero)
                VALUES
                (@Calle, @Ciudad, @Numero);

                SELECT LAST_INSERT_ID();";

            return connection.QuerySingle<int>(sql, direccion);
        }
      public void Actualizar(Direccion direccion)
        {
            using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                UPDATE Direccion
                SET calle = @Calle,
                    ciudad = @Ciudad,
                    numero = @Numero
                WHERE idDireccion = @IdDireccion";

            connection.Execute(sql, direccion);
        }
        public void Eliminar(int idDireccion)
        {
            using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                DELETE FROM Direccion
                WHERE idDireccion = @idDireccion";

            connection.Execute(sql, new { idDireccion });
        }



}