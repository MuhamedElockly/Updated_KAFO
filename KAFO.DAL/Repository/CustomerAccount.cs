
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class CustomerAccount : Repository<CustomerAccount>, ICustomerAccountRepository
    {
        private readonly AppDBContext _dbcontext;
        public CustomerAccount(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
