using System.Data;

namespace Bookify.Application.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateDbConnection();
}