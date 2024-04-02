namespace Bookify.Application.Abstractions.Authentication;

public interface IBookifyAuthenticationService
{
    Task<string> AuthenticateAsync(Domain.Users.User user, string password, CancellationToken cancellationToken = default);
}