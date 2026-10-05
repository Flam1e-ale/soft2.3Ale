using System.Data;
using MySqlConnector;
namespace Persistencia.Conexion;

    public class MySQLConnectionFactory:IDbConnectionFactory
    {
        private readonly string connectionString;
        private readonly string adminConnectionString;

        
         public MySQLConnectionFactory(string connectionString,string adminConnectionString)
        {
        this.connectionString = connectionString;
        this.adminConnectionString=adminConnectionString;
        }

        public IDbConnection CrearConexion()
        {
        return new MySqlConnection(connectionString);
        }
        public IDbConnection CrearAdminConnecion()
        {
            return new MySqlConnection(adminConnectionString);
        }
}
