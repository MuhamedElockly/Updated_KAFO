using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using KAFO.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.BLL.Managers
{
	public class InventoryManager
	{
		IUnitOfWork UOW = null;
		public InventoryManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
		//public List<dynamic> GetSellerInvetoryToday()
		//{
		//	DateTime today = DateTime.Today;
		//	var totalCashAdnCreditInvoices = UOW.Invoices.GetAll("User", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
		//		.Where(i => !i.IsDeleted && i.CreatedAt ==today).GroupBy(i => i.User.PhoneNumber)
		//		.Select(group => new { SellerName = group.Key, TotalCashInvoices = group.Where(i=>i.Type==InvoiceType.Cash).Sum(i => i.TotalInvoice),TotalCreditInvoices = group.Where(i => i.Type == InvoiceType.Credit).Sum(i => i.TotalInvoice) })
		//		.ToList();



		//}
		public List<SellerInventoryVM> GetSellerInvetoryToday()
		{
			DateTime today = DateTime.Today;
			var allInvoices = UOW.Invoices.GetAll("User")
				.Where(i => !i.IsDeleted && i.CreatedAt.Date == today && i.User != null)
				.ToList();
			var allTerminateInvoices = UOW.CreditTerminateInvoice.GetAll("User")
				.Where(i => !i.IsDeleted && i.CreatedAt.Date == today && i.User != null)
				.ToList();
			var allWithdrawInvoices = UOW.CreditWithdrawInvoices.GetAll("User")
				.Where(i => !i.IsDeleted && i.CreatedAt.Date == today && i.User != null)
				.ToList();
			var allCashReturnInvoices = UOW.Invoices.GetAll("User")
				.Where(i => !i.IsDeleted && i.CreatedAt.Date == today && i.Type == InvoiceType.CashReturn && i.User != null)
				.ToList();
			var allCreditReturnInvoices = UOW.Invoices.GetAll("User")
				.Where(i => !i.IsDeleted && i.CreatedAt.Date == today && i.Type == InvoiceType.CreditReturn && i.User != null)
				.ToList();
			var sellerGroups = allInvoices.Where(i => i.User != null && i.User.PhoneNumber != null && i.User.Name != null && i.User.Role == "seller")
				.GroupBy(i => new { i.User.Name, i.User.PhoneNumber });


			var result = sellerGroups.Select(group =>
			{
				var totalCash = group.Where(i => i.Type == InvoiceType.Cash).Sum(i => i.TotalInvoice);
				var totalCredit = group.Where(i => i.Type == InvoiceType.Credit).Sum(i => i.TotalInvoice);
				var totalCashReturn = allCashReturnInvoices
					.Where(t => t.User.PhoneNumber == group.Key.PhoneNumber)
					.Sum(t => t.TotalInvoice);
				var totalCreditReturn = allCreditReturnInvoices
				.Where(t => t.User.PhoneNumber == group.Key.PhoneNumber)
					.Sum(t => t.TotalInvoice);
				var totalTerminate = allTerminateInvoices
					.Where(t => t.User.PhoneNumber == group.Key.PhoneNumber)
					.Sum(t => t.TotalInvoice);
				var totalWithdraw = allWithdrawInvoices
					.Where(t => t.User.PhoneNumber == group.Key.PhoneNumber)
					.Sum(t => t.TotalInvoice);
				var totalSupply = totalCash + totalTerminate - totalWithdraw - totalCashReturn;

				return new SellerInventoryVM
				{
					SellerName = group.Key.Name,
					Phone = group.Key.PhoneNumber,
					TotalCashPayment = totalCash,
					TotalCreditPayment = totalCredit,
					TotalCashReturn = totalCashReturn,
					TotalCreditReturn = totalCreditReturn,
					TotalRefundCredit = totalTerminate,
					TotalSupplyMoney = totalSupply,
					TotalWithdrawCredit = totalWithdraw

				};
			}).ToList();

			return result;
		}

		public List<ProductRemainVM> GetProductInventoryToday()
		{

			var products = UOW.Products.GetAll("Category")
				.Where(p => !p.IsDeleted && p.StockQuantity > 0 && p.IsActive)
				.ToList();

			var result = products.Select(product =>
			{

				int totalRemain = (int)product.StockQuantity;

				int remainBox = 0;
				int remainPlus = 0;
				if (product.BoxQuantity == 0 || totalRemain == 0)
				{
					remainBox = 0;
					remainPlus = 0;
				}
				else
				{
					remainBox = totalRemain / (int)product.BoxQuantity;
					remainPlus = totalRemain % (int)product.BoxQuantity;

				}

				return new ProductRemainVM
				{
					ProductImage = product.ImageUrl,
					ProductName = product.Name,
					RemainBox = remainBox,
					RemainPlus = remainPlus,
					TotalRemain = totalRemain,
					ItemsPerBox = (int)product.BoxQuantity
				};
			}).ToList();

			return result;
		}
	}
}
