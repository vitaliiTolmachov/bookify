﻿using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }
    
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userId, out Guid userIdParsed)?
            userIdParsed :
            throw new ApplicationException("User identity is unavailable");
    }
}