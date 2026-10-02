using System.Data;
using MySqlConnector;
namespace Persistencia.Conexion;

    public class MySQLConnectionFactory:IDBConnectionFactory
    {
        private readonly string connectionString;

         public MySQLConnectionFactory(string connectionString)
        {
        this.connectionString = connectionString;
        }

        public IDbConnection CrearConexion()
        {
        return new MySqlConnection(connectionString);
        }
}
