
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class CashInvoiceRepsitory : Repository<CashInvoice>, ICashInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public CashInvoiceRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
