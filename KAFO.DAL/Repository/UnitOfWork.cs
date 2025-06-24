using Kafo.DAL.Data;

namespace Kafo.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDBContext _dbcontext;
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }

        public ICustomerAccountRepository CustomerAccounts { get; private set; }

        public IInvoiceItemRepository InvoiceItems { get; private set; }

        public IInvoiceRepository Invoices { get; private set; }

        public IUserRepository Users { get; private set; }

        public UnitOfWork(AppDBContext dbContext)
        {
            _dbcontext = dbContext;
            Categories = new CategoryRepository(_dbcontext);
            Products = new ProductRepsitory(_dbcontext);

            CustomerAccounts = new CustomerAccountRepsitory(_dbcontext);
            InvoiceItems = new InvoiceItemRepsitory(_dbcontext);
            Invoices = new InvoiceRepository(_dbcontext);
            Users = new UserRepository(_dbcontext);
        }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
