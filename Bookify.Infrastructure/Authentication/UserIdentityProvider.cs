using Bookify.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bookify.Infrastructure.Authentication;

internal sealed class UserIdentityProvider : IUserIdentityProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetIdentityId()
    {
        return _httpContextAccessor
                .HttpContext?
                .User
                .GetIdentityId() ??
            throw new ApplicationException("User context is unavailable");
    }
    
    public Guid GetIUserId()
    {
        return _httpContextAccessor
                   .HttpContext?
                   .User
                   .GetUserId() ??
               throw new ApplicationException("UserId is unavailable");
    }
}