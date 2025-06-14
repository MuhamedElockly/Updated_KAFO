using KAFO.Domain.Products;
using KAFO.Domain.Statics;

namespace KAFO.Domain.Invoices
{
    public class InvoiceItem : Base
    {
        public int Id { private set; get; }
        public Invoice Invoice { get; private set; } = null!;
        public Product Product { get; private set; }
        public decimal UnitSellingPrice => Product.SellingPrice;
        public decimal UnitPurchasingPrice => Product.LastPurchasingPrice;
        public decimal Quantity { get; private set; }

        private InvoiceItem()
        {

        }
        public InvoiceItem(Invoice invoice, Product product, decimal quantity)
        {
            if (invoice == null || product == null || quantity <= 0)
                throw new ArgumentNullException(Messages.ArgumentNullException);

            Invoice = invoice;
            Product = product;
            Quantity = quantity;
        }
        public void IncreaseQuantity(decimal quantity)
        {
            if (quantity > 0)
                Quantity += quantity;
            else
                throw new Exception(Messages.NegativeQuantityException);
        }
        public void DecreaseQuantity(decimal quantity)
        {
            if (quantity > 0 && ( Quantity - quantity ) >= 0)
                Quantity -= quantity;
            else
                throw new Exception(Messages.NegativeQuantityException);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            InvoiceItem other = (InvoiceItem) obj;
            return other.Product == Product;
        }

        public override int GetHashCode()
        {
            return Invoice.GetHashCode()
                + Product.GetHashCode()
                + UnitSellingPrice.GetHashCode()
                + UnitPurchasingPrice.GetHashCode()
                + Quantity.GetHashCode();
        }
    }
}
