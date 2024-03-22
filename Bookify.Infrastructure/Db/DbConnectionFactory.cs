﻿using System.Data;
using Bookify.Application.Data;
using Microsoft.Data.SqlClient;

namespace Bookify.Infrastructure.Db;

internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    internal DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<IDbConnection> CreateDbConnection()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}