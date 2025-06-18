using KAFO.Domain.Users;

namespace KAFO.Domain.Invoices
{
    public class CashInvoice : Invoice
    {
        public CashInvoice()
        {

        }
        public CashInvoice(DateTime createdAt, User user) : base(createdAt, user)
        {

        }
        public override void CompleteInvoice()
        {
            base.CompleteInvoice();
            foreach (var item in this.Items)
            {
                // Decrease The StockQuantity 
                item.Product.DecreaseStockQuantity(item.Quantity);
            }
        }
    }

}
