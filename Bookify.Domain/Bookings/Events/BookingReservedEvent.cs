using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingReservedEvent(Guid Id) : IDomainEvent;