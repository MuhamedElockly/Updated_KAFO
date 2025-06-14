
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class CashInvoice : Repository<CashInvoice>, ICashInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public CashInvoice(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
