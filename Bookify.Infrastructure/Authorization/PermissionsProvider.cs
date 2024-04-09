using Bookify.Domain.Users;
using Bookify.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionsProvider
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionsProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HashSet<string>> GetUserPermissions(string identityId)
    {
        //TODO: Add caching
        var permissions = await _dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .SelectMany(u => u.Roles.Select(r => r.Permissions))
            .FirstAsync();

        return permissions.Select(p => p.Name).ToHashSet();
    }
}