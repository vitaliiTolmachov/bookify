using Bookify.Application.Abstractions.Email;

namespace Bookify.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Users.Email email, string subject, string body, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}