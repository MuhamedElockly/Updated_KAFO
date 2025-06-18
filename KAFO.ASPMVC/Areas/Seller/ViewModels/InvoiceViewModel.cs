using KAFO.Domain.Invoices;
using KAFO.Domain.Statics;
using KAFO.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Models
{
    public enum InvoiceType
    {
        [Display(Name = "فاتورة نقدية")]
        Cash,
        [Display(Name = "فاتورة اجل")]
        Credit,
        [Display(Name = "فاتورة شراء")]
        Purchasing,
        [Display(Name = "فاتورة مرتجع نقدي")]
        CashReturn,
        [Display(Name = "فاتورة مرتجع اجل")]
        CreditReturn,
        [Display(Name = "فاتورة مرتجع شراء")]
        PurchasingReturn
    }
    public class InvoiceViewModel : Invoice
    {

        public CustomerAccount? CustomerAccount { get; set; }
        [Required(ErrorMessage = Messages.RequiredMessage)]
        public InvoiceType Type { get; set; }

        public InvoiceViewModel()
        {

        }
        public InvoiceViewModel(CashInvoice cashInvoice)
        {
            updateProps(cashInvoice);
            Type = InvoiceType.Cash;
        }

        public InvoiceViewModel(CreditInvoice creditInvoice)
        {
            updateProps(creditInvoice);
            this.CustomerAccount = creditInvoice.CustomerAccount;
            Type = InvoiceType.Credit;
        }

        private void updateProps(Invoice invoice)
        {
            this.Id = invoice.Id;
            this.CreatedAt = invoice.CreatedAt;
            this.Items = invoice.Items;
            this.User = invoice.User;
            this.TotalInvoice = invoice.TotalInvoice;
        }


    }
    public static class InvoiceViewModelHelper
    {
        static public CashInvoice ToCashInvoice(this InvoiceViewModel invoice)
        {
            return new CashInvoice()
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items,
                User = invoice.User,
                TotalInvoice = invoice.TotalInvoice
            };
        }
        static public CreditInvoice ToCreditInvoice(this InvoiceViewModel invoice)
        {
            return new CreditInvoice()
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items,
                User = invoice.User,
                TotalInvoice = invoice.TotalInvoice,
                CustomerAccount = invoice.CustomerAccount
            };
        }
        static public PurchasingInvoice ToPurchasingInvoice(this InvoiceViewModel invoice)
        {
            return new PurchasingInvoice()
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                Items = invoice.Items,
                User = invoice.User,
                TotalInvoice = invoice.TotalInvoice,
            };
        }
    }
}
