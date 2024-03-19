using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingCancelledEvent(Guid Id) : IDomainEvent;