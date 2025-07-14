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
} 