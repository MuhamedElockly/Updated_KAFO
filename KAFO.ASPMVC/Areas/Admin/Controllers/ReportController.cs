using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
