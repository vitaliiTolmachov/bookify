using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartment;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class BookingsTests : BaseTest
{
    private static PricingService PricingService = new PricingService();
    
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var duration = new Duration(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var apartment = ApartmentData.Create(price);
        
        // Act
        var booking = Booking.Reserve(user.Id, apartment, duration, DateTime.UtcNow, PricingService);
        
        // Assert
        var bookingReservedDomainEvent = AssertDomainEventWasPublished<BookingReservedEvent>(booking);
        bookingReservedDomainEvent.BookingId.Should().Be(booking.Id);
        apartment.LastBookedOnUtc.Should().Be(booking.CreatedOnUtc);
    }
    
    [Fact]
    public void Reserve_Should_UpdateLastBookedOnUtcForApartment()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var duration = new Duration(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
        var apartment = ApartmentData.Create(price);
        
        // Act
        var booking = Booking.Reserve(user.Id, apartment, duration, DateTime.UtcNow, PricingService);
        
        // Assert
        apartment.LastBookedOnUtc.Should().Be(booking.CreatedOnUtc);
    }
}