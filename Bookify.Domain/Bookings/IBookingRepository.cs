using Bookify.Domain.Apartments;

namespace Bookify.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Add(Booking domainEntity, CancellationToken cancellationToken = default);
    
    Task<bool> IsOverlappingAsync(
        Apartment apartment,
        Duration duration,
        CancellationToken cancellationToken = default);
}