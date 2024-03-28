using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Db.Repositories;

internal sealed class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public Task<bool> IsOverlappingAsync(Apartment apartment, Duration duration, CancellationToken cancellationToken = default)
    {
        return DbContext
            .Set<Booking>()
            .AnyAsync(
                booking =>
                    booking.ApartmentId == apartment.Id &&
                    booking.Duration.Start <= duration.End &&
                    booking.Duration.End >= duration.Start &&
                    BookingStatusExtensions.ActiveBookingStatuses.Contains((int)booking.Status),
                cancellationToken);
    }
}