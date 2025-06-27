using System;

namespace KAFO.Domain.ViewModels
{
    public class SellerInventoryVM
    {
        public string SellerName { get; set; }
        public string Phone { get; set; }
        public decimal TotalCashPayment { get; set; }
        public decimal TotalCreditPayment { get; set; }
        public decimal TotalRefundCredit { get; set; }
        public decimal TotalSupplyMoney { get; set; }
    }
} 