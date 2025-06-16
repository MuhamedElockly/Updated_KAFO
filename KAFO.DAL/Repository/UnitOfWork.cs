using Kafo.DAL.Data;


namespace Kafo.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _dbcontext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICashInvoiceRepository CashInvoice { get; private set; }
        public ICreditInvoiceRepository CreditInvoice { get; private set; }
        public ICustomerAccountRepository CustomerAccount { get; private set; }
        public IInvoiceItemRepository InvoiceItem { get; private set; }
        public IInvoiceRepository Invoice { get; private set; }

        public UnitOfWork(AppDBContext dbContext)
        {
            _dbcontext = dbContext;
            Category = new CategoryRepository(_dbcontext);
            Product = new ProductRepsitory(_dbcontext);
            CashInvoice = new CashInvoice(_dbcontext);
            CreditInvoice = new CreditInvoice(_dbcontext);
            CustomerAccount = new CustomerAccount(_dbcontext);
            InvoiceItem = new InvoiceItem(_dbcontext);
            Invoice = new InvoiceRepository(_dbcontext);
        }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
