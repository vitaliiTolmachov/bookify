namespace Bookify.Api.Controllers.Users;

public sealed record UserRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);