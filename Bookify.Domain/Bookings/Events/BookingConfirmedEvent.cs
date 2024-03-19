using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingConfirmedEvent(Guid Id) : IDomainEvent;