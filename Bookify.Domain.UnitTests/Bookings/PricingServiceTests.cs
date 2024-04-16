using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartment;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    private static readonly PricingService PricingService = new PricingService();
    
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var period = new Duration(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var expectedTotalPrice = new Money(price.Amount * period.Days, price.Currency);
        var apartment = ApartmentData.Create(price);

        // Act
        var pricingDetails = PricingService.CalculatePricingDetails(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var cleaningFee = new Money(99.99m, Currency.Usd);
        var period = new Duration(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var expectedTotalPrice = new Money((price.Amount * period.Days) + cleaningFee.Amount, price.Currency);
        var apartment = ApartmentData.Create(price, cleaningFee);

        // Act
        var pricingDetails = PricingService.CalculatePricingDetails(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
}