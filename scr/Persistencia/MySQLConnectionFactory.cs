using System.Data;
using MySqlConnector;
namespace Persistencia.Conexion;

    public class MySQLConnectionFactory:IDbConnectionFactory
    {
        private readonly string _connectionString;
        private readonly string _adminConnectionString;

        
         public MySQLConnectionFactory(string connectionString,string adminConnectionString)
        {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _adminConnectionString=adminConnectionString ?? throw new ArgumentNullException(nameof(adminConnectionString));
        }

        public IDbConnection CrearConexion()
        {
        return new MySqlConnection(_connectionString);
        }
        public IDbConnection CrearConexionAdmin()
        {
            return new MySqlConnection(_adminConnectionString);
        }
}
