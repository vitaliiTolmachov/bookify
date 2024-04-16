﻿using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingConfirmedEvent(Guid BookingId) : IDomainEvent;