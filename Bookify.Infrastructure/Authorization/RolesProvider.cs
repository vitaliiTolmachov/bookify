using Bookify.Domain.Users;
using Bookify.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class RolesProvider
{
    private readonly ApplicationDbContext _dbContext;

    public RolesProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetIdentityRolesAsync(string identityId)
    {
        var userRoles = await _dbContext.Set<User>()
            .Where(x => x.IdentityId == identityId)
            .Select(x => new UserRolesResponse
            {
                UserId = x.Id,
                Roles = x.Roles.ToList()
            }).FirstAsync();

        return userRoles;
    }
}