namespace Bookify.Application.User.GetLoggedIn;

public sealed record UserResponse(Guid Id, string Email, string Name);