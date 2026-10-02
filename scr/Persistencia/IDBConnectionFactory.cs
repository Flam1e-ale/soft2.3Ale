using System.Data;
using MySqlConnector;
namespace Persistencia.Conexion;

    public interface IDBConnectionFactory
    {   
        IDbConnection CrearConexion();
    }
