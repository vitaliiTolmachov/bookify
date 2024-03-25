using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public class Booking : Entity
{
    private Booking(Guid id,
        Guid userId,
        Guid apartmentId,
        Duration duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        DateTime createdOnUtc)
        : base(id)
    {
        UserId = userId;
        ApartmentId = apartmentId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        CreatedOnUtc = createdOnUtc;
        Status = BookingStatus.Reserved;
    }

    private Booking()
    {
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public Duration Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Guid userId,
        Apartment apartment,
        Duration duration,
        DateTime createdOnUtc,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePricingDetails(apartment, duration);
        var booking = new Booking(
            Guid.NewGuid(),
            userId,
            apartment.Id,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            createdOnUtc);
        
        booking.RaiseDomainEvent(new BookingReservedEvent(booking.Id));

        apartment.LastBookedOnUtc = booking.CreatedOnUtc;
        
        return booking;
    }

    public Result Confirm(DateTime confirmedOnUtc)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = confirmedOnUtc;
        
        base.RaiseDomainEvent(new BookingConfirmedEvent(this.Id));
        return Result.Success();
    }

    public Result Reject(DateTime rejectedOnUtc)
    {
        if(Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Rejected;
        RejectedOnUtc = rejectedOnUtc;
        
        base.RaiseDomainEvent(new BookingRejectedEvent(this.Id));
        return Result.Success();
    }

    public Result Complete(DateTime completedOnUtc)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        Status = BookingStatus.Completed;
        CompletedOnUtc = completedOnUtc;
        
        base.RaiseDomainEvent(new BookingCompletedEvent(this.Id));
        
        return Result.Success();
    }

    public Result Cancel(DateTime cancelledOnUtc)
    {
        if(Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);
        
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (currentDate > Duration.Start)
            return Result.Failure(BookingErrors.AlreadyStarted);

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = cancelledOnUtc;
        
        base.RaiseDomainEvent(new BookingCancelledEvent(this.Id));
        return Result.Success();
    }
}