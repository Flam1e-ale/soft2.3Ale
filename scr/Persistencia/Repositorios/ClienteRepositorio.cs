using Persistencia.Entidades;
using Persistencia.Conexion;
using Persistencia.Interfaces;
using MySqlConnector;
using Dapper;
using System.Data.Common;
namespace Persistencia.Repositorios;


public class ClienteRepositorio : IClienteRepositorio
{   
    private readonly IDbConnectionFactory _connectionFactory;
    public ClienteRepositorio(IDbConnectionFactory ConnectionFactory)
    {
        _connectionFactory=ConnectionFactory;
    }  
    public Cliente? ObtenerPorId(int idCliente )
    {
         using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                SELECT *
                FROM Cliente
                WHERE idCliente = @idCliente";
            return connection.QueryFirstOrDefault<Cliente> 
            (sql, new {idCliente});
        
    }
    public List<Cliente> ObtenerTodos()
        {
             using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                SELECT *
                FROM Cliente";

            return connection.Query<Cliente>(sql).ToList();
        }
     public int Crear(Cliente cliente)
        {
             using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                INSERT INTO Cliente (nombre, email)
                VALUES (@Nombre, @Email);

                SELECT LAST_INSERT_ID();";

            return connection.QuerySingle<int>(sql, cliente);
        }
      public void Actualizar(Cliente cliente)
        {
             using var connection = _connectionFactory.CrearConexion();

            string sql = @"
                UPDATE Cliente
                SET nombre = @Nombre,
                    email = @Email
                WHERE idCliente = @IdCliente";

            connection.Execute(sql, cliente);
        }
     public void Eliminar(int idCliente)
        {
            using var connection = _connectionFactory.CrearConexion();
            string sql = @"
                DELETE FROM Cliente
                WHERE idCliente = @idCliente";

            connection.Execute(sql, new { idCliente });
        }

}