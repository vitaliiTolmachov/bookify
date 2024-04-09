using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionAuthRequirement : IAuthorizationRequirement
{
    public PermissionAuthRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}