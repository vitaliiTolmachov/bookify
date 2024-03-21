namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    //Note: parameters after email can be wrapped into record
    Task SendAsync(Domain.Users.Email email, string subject, string body, CancellationToken cancellationToken = default);
}