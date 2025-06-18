
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class PurchasingInvoiceRepository : Repository<PurchasingInvoice>, IPurchasingInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public PurchasingInvoiceRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
