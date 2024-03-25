using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Db.Repositories;

public class GenericRepository<T> where T:Entity
{
    protected readonly ApplicationDbContext DbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public Task<T?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public void AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Add(entity);
    }
}