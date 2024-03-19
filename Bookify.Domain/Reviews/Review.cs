using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Reviews;

public sealed class Review : Entity
{
    private Review(
        Guid id,
        Guid apartmentId,
        Guid bookingId,
        Guid userId,
        Rating rating,
        Comment comment)
        : base(id)
    {
        ApartmentId = apartmentId;
        BookingId = bookingId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedOnUtc = DateTime.UtcNow;
    }
    
    public Guid ApartmentId { get; private set; }
    
    public Guid BookingId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Rating Rating { get; private set; }
    
    public Comment Comment { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public static Result<Review> Create(
        Booking booking,
        Rating rating,
        Comment comment)
    {
        if (booking.Status != BookingStatus.Completed)
            return Result<Review>.Failure(ReviewErrors.NotEligible);

        var review = new Review(
            Guid.NewGuid(),
            booking.ApartmentId,
            booking.Id,
            booking.UserId,
            rating,
            comment);
        
        review.RaiseDomainEvent(new ReviewCreatedEvent(review.Id));

        return review;
    }
}