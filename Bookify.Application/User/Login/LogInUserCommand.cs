using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.User.Login;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;