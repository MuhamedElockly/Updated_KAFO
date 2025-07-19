using Microsoft.AspNetCore.Mvc;
using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using System;
using System.Linq;
using System.Threading.Tasks;
using KAFO.BLL.Managers;
using Microsoft.AspNetCore.Authorization;

namespace KAFO.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
	{
		private readonly ReportManager _reportManager;
		public ReportController(ReportManager invoiceManager)
		{
			_reportManager = invoiceManager;

		}


		[HttpGet]
		public IActionResult GetProfitReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			double totalProfit = _reportManager.GetTotalProfit(startDate, endDate);
			return Json(new { totalProfit });
		}
		[HttpGet]
		public IActionResult GetMostSoldProductsReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var mostSoldProudct = _reportManager.GetMostProductSold(startDate, endDate);

			return Json(mostSoldProudct);
		}
		[HttpGet]
		public IActionResult GetSalesReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var totalSellsRes = _reportManager.GetTotalSells(startDate, endDate);

			return Json(new { totalSales = totalSellsRes });
		}
		[HttpGet]
		public IActionResult GetMostProfitableSellerReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var mostProftableSellers = _reportManager.GetMostProftableSeller(startDate, endDate);

			return Json(mostProftableSellers);
		}
		[HttpGet]
		public IActionResult GetMostProfitableProductReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var mostProftableProduct = _reportManager.GetMostProftableProduct(startDate, endDate);

			return Json(mostProftableProduct);
		}
		[HttpGet]
		public IActionResult GetLeastSoldProductsReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var leastSoldProduct = _reportManager.GetLeastProductSold(startDate, endDate);
			return Json(leastSoldProduct);
		}
		[HttpGet]
		public IActionResult GetTotalPaymentsReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var res = _reportManager.GetTotalSoldInvoices(startDate, endDate);
			return Json(new { totalSoldInvoices=res });
		}
		[HttpGet]
		public IActionResult GetTotalPurchaseInvoicesReport(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				Response.StatusCode = 400;
				return Json(new { message = "تاريخ البدء يجب أن يكون أقل من تاريخ الانتهاء" });
			}
			var totalPurchaseAmount = _reportManager.GetTotalPurchaseInvoicesAmount(startDate, endDate);
			return Json(new { totalPurchaseAmount });
		}
		[HttpGet]
		public IActionResult GetStockMoneyReport()
		{
			var totalStockMoney = _reportManager.GetTotalStockMoney();
			return Json(new { totalStockMoney });
		}
		[HttpGet]
		public IActionResult GetExpectedProfitsReport()
		{
			var totalExpectedProfits = _reportManager.GetTotalExpectedProfits();
			return Json(new { totalExpectedProfits });
		}



	}
}
