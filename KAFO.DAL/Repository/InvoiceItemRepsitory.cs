
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class InvoiceItemRepsitory : Repository<InvoiceItem>, IInvoiceItemRepository
    {
        private readonly AppDBContext _dbcontext;
        public InvoiceItemRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
