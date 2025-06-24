namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class InvoicesTableVM
    {
        public List<InvoiceVM> Invoices { get; set; } = new List<InvoiceVM>();
        public int CurrentInvoicePage { get; set; }
        public int TotalInvoicesPages { get; set; }
        public string InvoiceType { get; set; } // "sell" or "purchase"
    }
} 