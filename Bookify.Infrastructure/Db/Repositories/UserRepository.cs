using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Db.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public override void Add(User entity, CancellationToken cancellationToken = default)
    {
        foreach (var role in entity.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(entity);
    }
}