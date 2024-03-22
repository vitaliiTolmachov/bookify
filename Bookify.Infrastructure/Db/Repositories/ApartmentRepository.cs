using Bookify.Domain.Apartments;

namespace Bookify.Infrastructure.Db.Repositories;

internal sealed class ApartmentRepository : GenericRepository<Apartment>, IApartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}