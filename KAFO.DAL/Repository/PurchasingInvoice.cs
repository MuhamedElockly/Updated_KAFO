
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class PurchasingInvoice : Repository<PurchasingInvoice>, IPurchasingInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public PurchasingInvoice(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
