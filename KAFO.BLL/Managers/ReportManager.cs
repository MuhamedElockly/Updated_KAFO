using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.BLL.Managers
{
	public class ReportManager
	{
		IUnitOfWork UOW = null;
		public ReportManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
		public double GetTotalProfit(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			var invoiceIds = UOW.Invoice.GetAll().
				Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Select(i => i.Id).ToList();
			var totalProfit = (double)UOW.InvoiceItem.GetAll().Where(i => !i.IsDeleted && invoiceIds.Contains(i.Id)).Sum(i => (i.UnitSellingPrice - i.UnitPurchasingPrice) * i.Quantity);
			return totalProfit;
		}
		public double GetTodayTotalProfit()
		{
			DateTime today = DateTime.Today;
			return GetTotalProfit(today, today);
		}

		public double GetTotalSells(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			var totalSells = UOW.Invoice.GetAll().Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Sum(i => i.TotalInvoice);
			return (double)totalSells;
		}
		public double GetTotalSellsToday()
		{
			DateTime today = DateTime.Today;
			return GetTotalSells(today, today);
		}
		public int GetTotalProductSold(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			List<InvoiceItem> totalItems = UOW.Invoice.GetAll().Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate).SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
			var totalProducts = totalItems.Sum(i => i.Quantity);
			return (int)totalProducts;
		}

		public int GetTotalProductSoldToday()
		{
			DateTime today = DateTime.Today;
			return GetTotalProductSold(today, today);
		}

	}
}
