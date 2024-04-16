﻿using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Apartment;

internal static class ApartmentData
{
    public static Apartments.Apartment Create(Money price, Money? cleaningFee = null) => new(
        Guid.NewGuid(),
        new Name("Test apartment"),
        new Description("Test description"),
        new Address("Country", "State", "ZipCode", "City", "Street"),
        price,
        cleaningFee ?? Money.Zero(price.Currency));
}