using KAFO.Domain.Invoices;
using KAFO.Domain.Users;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class InvoiceVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public decimal TotalInvoice { get; set; }
        public string InvoiceType { get; set; }
        public string CustomerName { get; set; }
        public int ItemsCount { get; set; }
    }
} 