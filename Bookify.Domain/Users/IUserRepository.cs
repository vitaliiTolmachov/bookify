namespace Bookify.Domain.Users;

public interface IUserRepository
{
    Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Add(User domainEntity, CancellationToken cancellationToken = default);
}