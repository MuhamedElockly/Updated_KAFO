
using Kafo.DAL.Data;
using KAFO.Domain.Users;

namespace Kafo.DAL.Repository
{
    public class CustomerAccountRepsitory : Repository<CustomerAccount>, ICustomerAccountRepository
    {
        private readonly AppDBContext _dbcontext;
        public CustomerAccountRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
