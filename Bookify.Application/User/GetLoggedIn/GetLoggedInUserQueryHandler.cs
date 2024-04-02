using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Data;
using Bookify.Domain.Shared;
using Dapper;

namespace Bookify.Application.User.GetLoggedIn;

public class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IUserIdentityProvider _userIdentityProvider;

    public GetLoggedInUserQueryHandler(
        IDbConnectionFactory dbConnectionFactory,
        IUserIdentityProvider userIdentityProvider)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _userIdentityProvider = userIdentityProvider;
    }
    
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        var identityId = _userIdentityProvider.GetIdentityId();
        using var connection = _dbConnectionFactory.CreateDbConnection();
        
        const string sql = """
                           SELECT
                               id AS Id,
                               first_name AS FirstName,
                               last_name AS LastName,
                               email AS Email
                           FROM users
                           WHERE identity_id = @IdentityId
                           """;
        
        var user = await connection.QuerySingleAsync<UserResponse>(
            sql,
            new
            {
                identityId
            });

        return user;
    }
}