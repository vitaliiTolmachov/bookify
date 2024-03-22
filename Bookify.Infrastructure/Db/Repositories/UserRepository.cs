using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Db.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    protected UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}