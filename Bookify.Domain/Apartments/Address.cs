﻿namespace Bookify.Domain.Apartments;

public record Address(
    string Country,
    string State,
    string Street,
    string ZipCode,
    string City);