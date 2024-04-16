using Bookify.Api.Controllers.Users;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static UserRegisterRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
}
