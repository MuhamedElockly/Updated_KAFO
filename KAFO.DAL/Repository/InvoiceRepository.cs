
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public InvoiceRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }


    }
}
