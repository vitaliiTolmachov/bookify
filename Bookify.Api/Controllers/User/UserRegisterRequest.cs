namespace Bookify.Api.Controllers.User;

public sealed record UserRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);