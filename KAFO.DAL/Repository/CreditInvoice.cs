
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class CreditInvoice : Repository<CreditInvoice>, ICreditInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public CreditInvoice(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
