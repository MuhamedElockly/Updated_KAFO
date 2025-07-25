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
        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون إجمالي المدفوع أكبر من أو يساوي صفر")]
        public decimal TotalPaid { get; set; }
        
        [Display(Name = "اجمالي ما عليه")]
        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون إجمالي المستحق أكبر من أو يساوي صفر")]
        public decimal TotalOwed { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string? Email { get; set; }
        
        // Balance should always be >= 0 (TotalPaid >= TotalOwed)
        public decimal Balance => Math.Max(0, TotalPaid - TotalOwed);
        
        // Get the actual balance (can be negative)
        public decimal ActualBalance => TotalPaid - TotalOwed;
        
        // Get the amount owed (always >= 0)
        public decimal AmountOwed => Math.Max(0, TotalOwed - TotalPaid);
        
        public virtual ICollection<Invoice> Invoices { get; set; } = [];
        public virtual ICollection<CreditTerminateInvoice> Deposits { get; set; } = [];
        public bool IsDeleted { get; set; } = false;

        public CustomerAccount()
        {

        }
        public CustomerAccount(string customerName, decimal totalPaid = 0)
        {
            CustomerName = customerName;
            TotalPaid = totalPaid;
        }
        
        public void NormalizeAccount()
        {
            if (TotalPaid > 0 && TotalOwed > 0)
            {
                if (TotalPaid >= TotalOwed)
                {
                    TotalPaid -= TotalOwed;
                    TotalOwed = 0;
                }
                else
                {
                    TotalOwed -= TotalPaid;
                    TotalPaid = 0;
                }
            }
        }

        // Method to add payment and ensure balance >= 0
        public void AddPayment(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("المبلغ يجب أن يكون أكبر من صفر");
                
            TotalPaid += amount;
            NormalizeAccount();
        }
        
        // Method to add debt and ensure TotalOwed >= 0
        public void AddDebt(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("المبلغ يجب أن يكون أكبر من صفر");
                
            TotalOwed += amount;
            NormalizeAccount();
        }
        
        // Method to settle debt (reduce TotalOwed)
        public void SettleDebt(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("المبلغ يجب أن يكون أكبر من صفر");
                
            if (amount > TotalOwed)
                throw new ArgumentException("لا يمكن تسوية مبلغ أكبر من المستحق");
                
            TotalOwed = Math.Max(0, TotalOwed - amount); // Ensure TotalOwed never goes below 0
            NormalizeAccount();
        }
     
        public void WithdrawBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("المبلغ يجب أن يكون أكبر من صفر");
                
            if (amount > Balance)
                throw new ArgumentException("لا يمكن سحب مبلغ أكبر من الرصيد المتاح");
                
            TotalPaid = Math.Max(0, TotalPaid - amount);
            NormalizeAccount();
        }
    }
}
