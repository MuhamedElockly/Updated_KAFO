using KAFO.Domain.Users;

namespace KAFO.Domain.Invoices
{
    public class PurchasingInvoice : Invoice
    {
        public PurchasingInvoice()
        {

        }
        public PurchasingInvoice(DateTime createdAt, User user) : base(createdAt, user)
        {

        }
        public override void CompleteInvoice()
        {
            base.CompleteInvoice();
            foreach (var item in Items)
            {
                // change price if changed
                if (item.UnitPurchasingPrice != item.Product.LastPurchasingPrice)
                {
                    item.Product.ChangePurchasingPriceAndQuantity(item.UnitPurchasingPrice, item.UnitSellingPrice, item.Quantity);
                }
                else // because the ChangePurchasingPriceAndQuantity will increase the quantity
                {
                    // update products Quantity
                    item.Product.IncreaseStockQuantity(item.Quantity);
                }
            }
        }
    }
}
