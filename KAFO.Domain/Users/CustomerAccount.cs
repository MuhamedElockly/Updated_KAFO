using KAFO.Domain.Invoices;
using System.ComponentModel.DataAnnotations;

namespace KAFO.Domain.Users
{
    public class CustomerAccount : Base, ISoftDelete
    {
        public int Id { get; set; }
        [Display(Name = "اسم العميل")]
        public string CustomerName { set; get; }
        [Display(Name = "اجمالي ما دفع")]
        public decimal TotalPaid { get; set; }
        [Display(Name = "اجمالي ما عليه")]
        public decimal TotalOwed { get; set; }
        public decimal Balance => TotalPaid - TotalOwed;
        public virtual ICollection<CreditInvoice> Invoices { get; set; } = [];
        public bool IsDeleted { get; set; } = false;

        public CustomerAccount()
        {

        }
        public CustomerAccount(string customerName, decimal totalPaid = 0)
        {
            CustomerName = customerName;
            TotalPaid = totalPaid;
        }
    }
}
