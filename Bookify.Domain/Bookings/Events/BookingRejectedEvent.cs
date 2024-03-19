using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingRejectedEvent(Guid Id) : IDomainEvent;