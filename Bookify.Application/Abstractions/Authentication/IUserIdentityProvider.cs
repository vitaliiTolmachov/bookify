namespace Bookify.Application.Abstractions.Authentication;

public interface IUserIdentityProvider
{
    string GetIdentityId();

    Guid GetIUserId();
}