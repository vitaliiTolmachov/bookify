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
        Money totalPrice)
        : base(id)
    {
        UserId = userId;
        ApartmentId = apartmentId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        CreatedOnUtc = DateTime.UtcNow;
        BookingStatus = BookingStatus.Reserved;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public Duration Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus BookingStatus { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Guid userId,
        Apartment apartment,
        Duration duration)
    {
        var pricingDetails = PricingService.CalculatePricingDetails(apartment, duration);
        
        var booking = new Booking(
            Guid.NewGuid(),
            userId,
            apartment.Id,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice);
        
        booking.RaiseDomainEvent(new BookingCreatedEvent(booking.Id));

        apartment.LastBookedOnUtc = booking.CreatedOnUtc;
        
        return booking;
    }
}