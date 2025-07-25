using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.DAL.Repository.IRepository;
using KAFO.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.DAL.Repository
{
    public class CreditWithdrawInvoiceRepository : Repository<CreditWithdrawInvoice>, ICreditWithdrawInvoice
    {

        private readonly AppDBContext _dbcontext;
        public CreditWithdrawInvoiceRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

    }
}
