
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class Invoice : Repository<Invoice>, IInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public Invoice(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
