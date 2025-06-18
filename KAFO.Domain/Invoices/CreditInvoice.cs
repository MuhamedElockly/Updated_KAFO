using KAFO.Domain.Users;

namespace KAFO.Domain.Invoices
{
    public class CreditInvoice : Invoice
    {
        public CustomerAccount CustomerAccount { get; set; }
        public CreditInvoice()
        {

        }
        public CreditInvoice(DateTime createdAt, User user, CustomerAccount customerAccount) : base(createdAt, user)
        {
            CustomerAccount = customerAccount;
        }
        public override void CompleteInvoice()
        {
            base.CompleteInvoice();
            foreach (var item in Items)
            {
                // Decrease The StockQuantity 
                item.Product.DecreaseStockQuantity(item.Quantity);
            }
            CustomerAccount.TotalOwed += TotalInvoice;
        }
    }

}
