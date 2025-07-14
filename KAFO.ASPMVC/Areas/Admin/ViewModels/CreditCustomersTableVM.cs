using System.Collections.Generic;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomersTableVM
    {
        public List<CreditCustomerDisplayVM> CreditCustomers { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
} 