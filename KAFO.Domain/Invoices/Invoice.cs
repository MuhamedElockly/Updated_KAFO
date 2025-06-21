using KAFO.Domain.Products;
using KAFO.Domain.Statics;
using KAFO.Domain.Users;
using System.ComponentModel.DataAnnotations;
namespace KAFO.Domain.Invoices
{
    public abstract class Invoice : Base, ISoftDelete
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        [Required]
        public required ICollection<InvoiceItem> Items { set; get; } = [];
        public User User { set; get; }
        public decimal TotalInvoice { set; get; }
        public bool IsDeleted { get; set; } = false;

        protected Invoice()
        {

        }
        public Invoice(DateTime createdAt, User user)
        {
            CreatedAt = createdAt;
            User = user;
        }
        private InvoiceItem? GetInvoiceItemByProduct(Product product)
        {
            return Items.Where(i => i.Product == product).FirstOrDefault();
        }

        public void AddInvoiceItem(Product product, decimal quantity)
        {
            var item = GetInvoiceItemByProduct(product);
            if (item != null)
            {
                item.IncreaseQuantity(quantity);
            }
            else
            {
                Items.Add(new InvoiceItem(this, product, quantity));
            }
        }

        /// <summary>
        /// remove product from invoice or chnage quantity of this product
        /// if you send quantity = -1 or not send the quantity you want to remove all this product from invoice not just decrease quantity
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity">if you send quantity = -1 or not send the quantity  you want to remove all this product from invoice not just decrease quantity</param>
        /// <exception cref="Exception"></exception>
        public void RemoveInvoiceItem(Product product, decimal quantity = -1)
        {
            var item = GetInvoiceItemByProduct(product);
            if (item != null)
            {
                if (quantity == -1)
                    Items.Remove(item);
                else
                    item.DecreaseQuantity(quantity);
            }
            else
                throw new Exception("cannot remove unfound InvoiceItem (product)");
        }

        public virtual void CompleteInvoice()
        {
            if (Items.Count < 1)
                throw new Exception(Messages.EmptyInvoice);
            if (TotalInvoice <= 0)
                CalculateTotalInvoice();
        }

        public decimal CalculateTotalInvoice()
        {
            foreach (var item in Items)
                TotalInvoice += ( item.UnitSellingPrice * item.Quantity );
            return TotalInvoice;
        }
    }

}
