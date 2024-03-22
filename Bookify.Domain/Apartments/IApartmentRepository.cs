namespace Bookify.Domain.Apartments;

public interface IApartmentRepository
{
    Task<Apartment?> FindAsync(Guid id, CancellationToken cancellationToken = default);
}