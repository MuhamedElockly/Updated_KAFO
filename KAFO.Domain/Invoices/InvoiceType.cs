using System.ComponentModel.DataAnnotations;

namespace KAFO.Domain.Invoices
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
}