using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews.Events;

public sealed record ReviewCreatedEvent(Guid Id) : IDomainEvent;