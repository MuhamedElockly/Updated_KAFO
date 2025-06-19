
using Kafo.DAL.Data;
using KAFO.Domain.Users;

namespace Kafo.DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDBContext _dbContext;
        public UserRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
