using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingCreatedEvent(Guid BookingId) : IDomainEvent;