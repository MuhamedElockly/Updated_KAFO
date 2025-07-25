using KAFO.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.Domain.Invoices
{
    public class CreditWithdrawInvoice
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }

        public User User { set; get; }
        public decimal TotalInvoice { set; get; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(CustomerAccount))]
        public int CustomerAccountId { get; set; }
        public CustomerAccount CustomerAccount { get; set; }
    }
}
