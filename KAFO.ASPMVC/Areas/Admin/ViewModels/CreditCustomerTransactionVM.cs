using System;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class CreditCustomerTransactionVM
	{
		public bool IsDeposit { get; set; } = true;
		public string SellerName { get; set; }
		public DateTime Time { get; set; }
		public decimal Amount { get; set; }
	}
}