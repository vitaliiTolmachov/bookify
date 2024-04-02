using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.User.GetLoggedIn;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>
{

}