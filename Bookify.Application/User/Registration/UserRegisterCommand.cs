using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.User.Registration;

public sealed record UserRegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
    ) : ICommand<Guid>
{
}