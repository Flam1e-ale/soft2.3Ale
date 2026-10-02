
using Persistencia.Entidades;
using Persistencia.Interfaces;
using MySqlConnector;
using Dapper;
using System.Data.Common;
namespace Persistencia.Repositorios;


public class ClienteRepositorio : IClienteRepositorio
{   
    private readonly string _connectionString;
    public ClienteRepositorio(string connetionString)
    {
        _connectionString=connetionString;
    }  
    public Cliente ObtenerPorId(int idCliente )
    {
        using var connection = new MySqlConnection(_connectionString);

            string sql = @"
                SELECT *
                FROM Cliente
                WHERE idCliente = @idCliente";
            return connection.QueryFirstOrDefault<Cliente> 
            (sql, new {idCliente});
        
    }
}