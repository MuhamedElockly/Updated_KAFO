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
		private readonly ReportManager _invcoiceManager;
		public ReportController(ReportManager invoiceManager)
		{
			_invcoiceManager = invoiceManager;

		}


		[HttpGet]
		public IActionResult GetProfitReport(DateTime startDate, DateTime endDate)
		{
			double totalProfit = _invcoiceManager.GetTotalProfit(startDate, endDate);
		//	double totalProfit = 345;
			return Json(new { totalProfit });
		}


	}
}
