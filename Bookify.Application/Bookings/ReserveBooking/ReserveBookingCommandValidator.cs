using FluentValidation;

namespace Bookify.Application.Bookings.ReserveBooking;

internal class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
{
    public ReserveBookingCommandValidator()
    {
        RuleFor(x => x.ApartmentId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);
    }
}