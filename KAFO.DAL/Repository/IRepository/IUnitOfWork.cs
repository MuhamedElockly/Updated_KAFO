namespace Kafo.DAL.Repository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICashInvoiceRepository CashInvoice { get; }
        ICreditInvoiceRepository CreditInvoice { get; }
        ICustomerAccountRepository CustomerAccount { get; }
        IInvoiceItemRepository InvoiceItem { get; }
        IInvoiceRepository Invoice { get; }

        void Save();
    }
}
