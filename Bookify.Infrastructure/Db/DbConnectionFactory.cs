using System.Data;
using Bookify.Application.Data;
using Npgsql;

namespace Bookify.Infrastructure.Db;

internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IDbConnection CreateDbConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}