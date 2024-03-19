using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;

namespace Bookify.Domain.Bookings;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<bool> IsOverlappingAsync(
        Apartment apartment,
        Duration duration,
        CancellationToken cancellationToken = default);
}