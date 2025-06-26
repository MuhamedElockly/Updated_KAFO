using Microsoft.AspNetCore.Mvc;
using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using System;
using System.Linq;
using System.Threading.Tasks;
using KAFO.BLL.Managers;

namespace KAFO.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
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
			double totalProfit = _reportManager.GetTotalProfit(startDate, endDate);
			return Json(new { totalProfit });
		}
		[HttpGet]
		public IActionResult GetMostSoldProductsReport(DateTime startDate, DateTime endDate)
		{
			var mostSoldProudct = _reportManager.GetMostProductSold(startDate, endDate);

			return Json(mostSoldProudct);
		}


	}
}
