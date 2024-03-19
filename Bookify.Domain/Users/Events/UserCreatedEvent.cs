using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public record UserCreatedEvent(Guid UserId) : IDomainEvent;