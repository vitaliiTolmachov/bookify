using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PricingService _pricingService;

    public ReserveBookingCommandHandler(
        IApartmentRepository apartmentRepository,
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        PricingService pricingService)
    {
        _apartmentRepository = apartmentRepository;
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _pricingService = pricingService;
    }
    
    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(request.UserId, cancellationToken);

        //Try to return Result.Failure from repository in case not found and call Get without check
        if (user == null)
            return Result<Guid>.Failure(UserErrors.NotFound(request.UserId));
        
        var apartment = await _apartmentRepository.FindAsync(request.ApartmentId, cancellationToken);
        
        if (apartment == null)
            return Result<Guid>.Failure(ApartmentErrors.NotFound(request.ApartmentId));

        var duration = new Duration(request.StartDate, request.EndDate);

        if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result<Guid>.Failure(BookingErrors.Overlap);
        }
        
        try
        {
            var booking = Booking.Reserve(user.Id, apartment, duration, _dateTimeProvider.CurrentTimeUtc, _pricingService);

            _bookingRepository.Add(booking, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        catch (ConcurrencyException ex)
        {
            //Since booking entity wasn't saved yet, we can rely on apartment entity's row version (See ApartmentConfiguration IsRowVersion)
            return Result<Guid>.Failure(BookingErrors.Overlap);
        }
    }
}