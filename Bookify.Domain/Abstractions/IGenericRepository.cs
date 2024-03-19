namespace Bookify.Domain.Abstractions;

public interface IGenericRepository<T> where T:Entity
{
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<T?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task AddAsync(T domainEntity, CancellationToken cancellationToken = default);
}