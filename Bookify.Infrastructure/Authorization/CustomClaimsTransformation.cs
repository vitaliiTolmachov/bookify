using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization;

/// <summary>
/// Since our auth token does not have roles,
/// here we can populate our roles from db to our JWT token to make [Authorize(Roles)] work
/// </summary>
internal sealed class CustomClaimsTransformation : IClaimsTransformation
{
    private readonly IServiceProvider _serviceProvider;

    public CustomClaimsTransformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(x => x.Type == ClaimTypes.Role) && //for role based authorization
            principal.HasClaim(x => x.Type == JwtRegisteredClaimNames.Sub)) //for resource based authorization
        {
            return principal;
        }

        using var scope = _serviceProvider.CreateScope();
        var rolesProvider = scope.ServiceProvider.GetRequiredService<RolesProvider>();
        var userRoles = await rolesProvider.GetIdentityRolesAsync(principal.GetIdentityId());

        var customClaims = new ClaimsIdentity();
        customClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));

        foreach (var role in userRoles.Roles)
        {
            customClaims.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }
        
        principal.AddIdentity(customClaims);
        return principal;
    }
}