﻿using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using FluentAssertions;

namespace Bookify.Application.IntegrationTests.Bookings;

public class ConfirmBookingTests : BaseIntegrationTest
{
    private static readonly Guid BookingId = Guid.NewGuid();

    public ConfirmBookingTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task ConfirmBooking_ShouldReturnFailure_WhenBookingIsNotFound()
    {
        // Arrange
        var command = new ConfirmBookingCommand(BookingId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Error.Should().Be(BookingErrors.NotFound(BookingId));
    }
}