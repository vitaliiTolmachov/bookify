using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Bookings.ReserveBooking;

public record ReserveBookingCommand(
    Guid UserId,
    Guid ApartmentId,
    DateOnly StartDate,
    DateOnly EndDate
    ) : ICommand<Guid>;