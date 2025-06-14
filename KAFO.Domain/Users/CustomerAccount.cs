using KAFO.Domain.Invoices;

namespace KAFO.Domain.Users
{
    public class CustomerAccount : Base
    {
        public int Id { get; set; }
        public string CustomerName { set; get; }
        public decimal TotalPaid { get; set; }
        public decimal TotalOwed { get; set; }
        public decimal Balance => TotalPaid - TotalOwed;
        public virtual ICollection<CreditInvoice> Invoices { get; set; } = [];

        private CustomerAccount()
        {

        }
        public CustomerAccount(string customerName, decimal totalPaid = 0)
        {
            CustomerName = customerName;
            TotalPaid = totalPaid;
        }
    }
}
