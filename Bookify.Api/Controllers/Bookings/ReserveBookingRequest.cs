﻿using System.ComponentModel.DataAnnotations;

namespace Bookify.Api.Controllers.Bookings;

public sealed class ReserveBookingRequest
{
    public Guid UserId { get; set; }
    public Guid ApartmentId { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}