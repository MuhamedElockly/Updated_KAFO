using KAFO.DAL.Repository.IRepository;

namespace Kafo.DAL.Repository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ICustomerAccountRepository CustomerAccounts { get; }
        IInvoiceItemRepository InvoiceItems { get; }
        IInvoiceRepository Invoices { get; }
        IUserRepository Users { get; }
        ICreditTerminateInvoice CreditTerminateInvoice { get; }

        void Save();
    }
}
