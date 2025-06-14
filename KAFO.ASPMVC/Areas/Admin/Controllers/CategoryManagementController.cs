using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryManagementController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
