using System;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomerTransactionVM
    {
        public string SellerName { get; set; }
        public DateTime Time { get; set; }
        public decimal DepositMoney { get; set; }
    }
} 