using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingCompletedEvent(Guid Id) : IDomainEvent;