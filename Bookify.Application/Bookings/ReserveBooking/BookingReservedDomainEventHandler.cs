using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking;

//MediatR pipeline responsible for that
internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmailService _emailService;

    public BookingReservedDomainEventHandler(
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _emailService = emailService;
    }
    
    public async Task Handle(BookingReservedEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.FindAsync(notification.Id, cancellationToken);

        if (booking == null)
            return;

        var user = await _userRepository.FindAsync(booking.UserId, cancellationToken);
        
        if(user == null)
            return;

        await _emailService.SendAsync(
            user.Email,
            "Confirm your booking",
            "You have 10 minutes to confirm your reservation",
            cancellationToken);
    }
}