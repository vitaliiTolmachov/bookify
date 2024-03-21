using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePricingDetails(Apartment apartment, Duration duration)
    {
        var currency = apartment.Price.Currency;
        var dailyPrice = apartment.Price.Amount * duration.Days;

        var priceForPeriod = new Money(dailyPrice, currency);
        decimal amenityPercentage = Decimal.Zero;

        foreach (Amenity amenity in apartment.Amenities)
        {
            amenityPercentage += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0m
            };
        }

        var amenityUpCharge = amenityPercentage > 0 ? 
            new Money(priceForPeriod.Amount * amenityPercentage, currency):
            new Money(0, currency);

        var totalPrice = Money.Zero(currency);

        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenityUpCharge;
        return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenityUpCharge, totalPrice);
    }
}