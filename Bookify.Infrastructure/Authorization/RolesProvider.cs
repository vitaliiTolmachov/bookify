using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class RolesProvider
{
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _dbContext;

    public RolesProvider(
        ICacheService cacheService,
        ApplicationDbContext dbContext)
    {
        _cacheService = cacheService;
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetIdentityRolesAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";
        var cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey);
        if (cachedRoles is not null)
        {
            return cachedRoles;
        }
        
        var dbRoles = await _dbContext.Set<User>()
            .Where(x => x.IdentityId == identityId)
            .Select(x => new UserRolesResponse
            {
                UserId = x.Id,
                Roles = x.Roles.ToList()
            }).FirstAsync();

        await _cacheService.SetAsync(cacheKey, dbRoles);

        return dbRoles;
    }
}