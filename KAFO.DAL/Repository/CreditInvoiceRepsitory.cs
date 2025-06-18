
using Kafo.DAL.Data;
using KAFO.Domain.Invoices;

namespace Kafo.DAL.Repository
{
    public class CreditInvoiceRepsitory : Repository<CreditInvoice>, ICreditInvoiceRepository
    {
        private readonly AppDBContext _dbcontext;
        public CreditInvoiceRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
