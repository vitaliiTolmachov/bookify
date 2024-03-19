using Bookify.Domain.Shared;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid id) => new(
        $"{nameof(User)}.{nameof(NotFound)}",
        $"The user with the specified id: {id} was not found");

    public static Error InvalidCredentials = new(
        $"{nameof(User)}.{nameof(InvalidCredentials)}",
        "The provided credentials were invalid");
}