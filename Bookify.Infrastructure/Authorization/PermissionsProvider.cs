using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionsProvider
{
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _dbContext;

    public PermissionsProvider(
        ICacheService cacheService,
        ApplicationDbContext dbContext)
    {
        _cacheService = cacheService;
        _dbContext = dbContext;
    }

    public async Task<HashSet<string>> GetUserPermissions(string identityId)
    {
        var cacheKey = $"auth:permissions-{identityId}";
        var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }
        
        var dbPermissions = await _dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .SelectMany(u => u.Roles.Select(r => r.Permissions))
            .FirstAsync();

        var uniquePermissions = dbPermissions.Select(p => p.Name).ToHashSet();

        await _cacheService.SetAsync(cacheKey, uniquePermissions);

        return uniquePermissions;
    }
}