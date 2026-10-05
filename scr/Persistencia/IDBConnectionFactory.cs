using System.Data;
using MySqlConnector;
namespace Persistencia.Conexion;

    public interface IDbConnectionFactory
    {   
        IDbConnection CrearConexion();
        IDbConnection CrearAdminConnecion();

    }
