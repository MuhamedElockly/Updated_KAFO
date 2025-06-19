using Kafo.DAL.Data;

namespace Kafo.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDBContext _dbcontext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICashInvoiceRepository CashInvoice { get; private set; }

        public ICreditInvoiceRepository CreditInvoice { get; private set; }

        public ICustomerAccountRepository CustomerAccount { get; private set; }

        public IInvoiceItemRepository InvoiceItem { get; private set; }

        public IInvoiceRepository Invoice { get; private set; }

        public IPurchasingInvoiceRepository PurchasingInvoice { get; private set; }

        public IUserRepository User { get; private set; }

        public UnitOfWork(AppDBContext dbContext)
        {
            _dbcontext = dbContext;
            Category = new CategoryRepository(_dbcontext);
            Product = new ProductRepsitory(_dbcontext);

            CashInvoice = new CashInvoiceRepsitory(_dbcontext);
            PurchasingInvoice = new PurchasingInvoiceRepository(_dbcontext);
            CreditInvoice = new CreditInvoiceRepsitory(_dbcontext);
            CustomerAccount = new CustomerAccountRepsitory(_dbcontext);
            InvoiceItem = new InvoiceItemRepsitory(_dbcontext);
            Invoice = new InvoiceRepository(_dbcontext);
            User = new UserRepository(_dbcontext);
        }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
