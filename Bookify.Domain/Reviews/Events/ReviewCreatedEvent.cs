using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews.Events;

public record ReviewCreatedEvent(Guid Id) : IDomainEvent;