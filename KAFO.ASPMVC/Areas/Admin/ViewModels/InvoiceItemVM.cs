namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class InvoiceItemVM
    {
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
} 