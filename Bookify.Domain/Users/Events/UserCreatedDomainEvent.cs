using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public record UserCreatedDomainEvent : IDomainEvent
{
    internal UserCreatedDomainEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
}