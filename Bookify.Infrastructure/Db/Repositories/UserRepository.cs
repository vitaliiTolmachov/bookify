using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Db.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}