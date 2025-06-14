
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class InvoiceItem : Repository<InvoiceItem>, IInvoiceItemRepository
    {
        private readonly AppDBContext _dbcontext;
        public InvoiceItem(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
