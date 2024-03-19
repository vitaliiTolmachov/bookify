using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Events;

public static class BookingErrors
{
    public static Error NotReserved =>
        new ($"{nameof(Booking)}{nameof(NotReserved)}", "The booking is not reserved");

    public static Error NotConfirmed =>
        new($"{nameof(Booking)}{nameof(NotConfirmed)}", "The booking is not confirmed");

    public static Error AlreadyStarted =>
        new($"{nameof(Booking)}{nameof(AlreadyStarted)}", "The booking is not already started");
    
    public static Error Overlap =>
        new($"{nameof(Booking)}{nameof(Overlap)}", "The booking is overlapping with existing one");
    
    public static Error NotFound(Guid id) =>
        new($"{nameof(Booking)}{nameof(NotFound)}", $"The booking with id: {id} not found");
}