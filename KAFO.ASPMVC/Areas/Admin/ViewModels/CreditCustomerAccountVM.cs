using System.Collections.Generic;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomerAccountVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Gender { get; set; }
        public decimal Balance { get; set; }
        public decimal Credit { get; set; }
        public List<CreditCustomerTransactionVM> Transactions { get; set; }=new List<CreditCustomerTransactionVM>() { };
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
} 