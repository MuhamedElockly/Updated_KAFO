using KAFO.Domain.Products;
using KAFO.Domain.Statics;
using KAFO.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KAFO.Domain.Invoices
{
    public class Invoice : Base, ISoftDelete
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public InvoiceType Type { get; set; }
        [Required]
        public ICollection<InvoiceItem> Items { set; get; } = [];
        
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { set; get; }
        
        public decimal TotalInvoice { set; get; }
        public bool IsDeleted { get; set; } = false;

        // credit invoice 

        [ForeignKey(nameof(CustomerAccount))]
        public int? CustomerAccountId { get; set; }
        public CustomerAccount? CustomerAccount { get; set; }

        // End of credit invoice

        public Invoice()
        {

        }
        public Invoice(DateTime createdAt, User user, InvoiceType type)
        {
            CreatedAt = createdAt;
            User = user;
            UserId = user.Id;
            Type = type;
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

            switch (Type)
            {
                case InvoiceType.Cash:
                    CashInvoiceCompleteInvoice();
                    break;
                case InvoiceType.Credit:
                    CreditInvoiceCompleteInvoice();
                    break;
                case InvoiceType.Purchasing:
                    PurchasingInvoiceCompleteInvoice();
                    break;
                case InvoiceType.CashReturn:
                    CashReturnInvoiceCompleteInvoice();
                    break;
                case InvoiceType.CreditReturn:
                    CreditReturnInvoiceCompleteInvoice();
                    break;
                case InvoiceType.PurchasingReturn:
                    PurchasingReturnInvoiceCompleteInvoice();
                    break;
                default:
                    break;
            }

            if (TotalInvoice <= 0)
                CalculateTotalInvoice();
        }

        private void PurchasingInvoiceCompleteInvoice()
        {
            foreach (var item in Items)
            {
                //// change price if changed
                //if (item.UnitPurchasingPrice != item.Product.LastPurchasingPrice)
                //{
                //    item.Product.ChangePurchasingPriceAndQuantity(item.UnitPurchasingPrice, item.UnitSellingPrice, item.Quantity);
                //}
                //else // because the ChangePurchasingPriceAndQuantity will increase the quantity
                //{
                //    // update products Quantity
                //    item.Product.IncreaseStockQuantity(item.Quantity);
                //}
                item.Product.ChangePurchasingPriceAndQuantity(item.Product.LastPurchasingPrice, item.UnitSellingPrice, item.Quantity);
                //item.Product.IncreaseStockQuantity(item.Quantity);
            }
        }

        private void CreditInvoiceCompleteInvoice()
        {
            foreach (var item in Items)
            {
                // Decrease The StockQuantity 
                item.Product.DecreaseStockQuantity(item.Quantity);
            }

            if (TotalInvoice <= 0)
                CalculateTotalInvoice();

            // Use available balance to pay off invoice
            if (CustomerAccount!.TotalPaid > 0)
            {
                if (CustomerAccount.TotalPaid >= TotalInvoice)
                {
                    // All paid from balance
                    CustomerAccount.TotalPaid -= TotalInvoice;
                    // No debt added
                }
                else
                {
                    // Partially paid from balance, rest is debt
                    var remaining = TotalInvoice - CustomerAccount.TotalPaid;
                    CustomerAccount.TotalPaid = 0;
                    CustomerAccount.AddDebt(remaining);
                }
            }
            else
            {
                // No balance, all is debt
                CustomerAccount.AddDebt(TotalInvoice);
            }
        }

        private void CashInvoiceCompleteInvoice()
        {
            foreach (var item in this.Items)
            {
                // Decrease The StockQuantity 
                item.Product.DecreaseStockQuantity(item.Quantity);
            }
        }
        private void PurchasingReturnInvoiceCompleteInvoice()
        {
            throw new NotImplementedException();
        }

        private void CreditReturnInvoiceCompleteInvoice()
        {
            throw new NotImplementedException();
        }

        private void CashReturnInvoiceCompleteInvoice()
        {
            throw new NotImplementedException();
        }

        public decimal CalculateTotalInvoice()
        {
            foreach (var item in Items)
                TotalInvoice += ( item.UnitSellingPrice * item.Quantity );
            return TotalInvoice;
        }
    }

}
