using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionAuthHandler : AuthorizationHandler<PermissionAuthRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionAuthHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var permissionsProvider = scope.ServiceProvider.GetRequiredService<PermissionsProvider>();
        var identityId = context.User.GetIdentityId();
        HashSet<string> permissions = await permissionsProvider.GetUserPermissions(identityId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}