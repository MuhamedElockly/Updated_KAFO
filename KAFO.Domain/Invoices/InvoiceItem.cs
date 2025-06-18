using KAFO.Domain.Products;
using KAFO.Domain.Statics;
using System.ComponentModel.DataAnnotations.Schema;

namespace KAFO.Domain.Invoices
{
    public class InvoiceItem : Base
    {
        public int Id { set; get; }

        [ForeignKey(nameof(Invoice))]
        public int InvoiceId { set; get; }

        [ForeignKey(nameof(Product))]
        public int ProductId { set; get; }
        public Invoice Invoice { get; set; } = null!;
        public Product Product { get; set; }
        public decimal UnitSellingPrice { get; set; }
        public decimal UnitPurchasingPrice { get; set; }
        public decimal Quantity { get; set; }

        public InvoiceItem()
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
