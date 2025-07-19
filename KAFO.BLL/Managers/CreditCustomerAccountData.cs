using System;
using System.Collections.Generic;

namespace KAFO.BLL.Managers
{
    public class CreditCustomerAccountData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Gender { get; set; }
        public decimal Balance { get; set; }
        public decimal Credit { get; set; }
        public List<CreditCustomerTransactionData> Transactions { get; set; }
    }

    public class CreditCustomerTransactionData
    {
        public string SellerName { get; set; }
        public DateTime Time { get; set; }
        public decimal DepositMoney { get; set; }
    }

    public class SettlementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public decimal NewTotalOwed { get; set; }
        public decimal NewTotalPaid { get; set; }
        public decimal OldTotalOwed { get; set; }
        public decimal OldTotalPaid { get; set; }
        public string CustomerName { get; set; }
        public decimal AmountPaid { get; set; }
    }
} 