
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class InvoiceRepsitory : Repository<Invoice>, IInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public InvoiceRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
