using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Controllers
{
    public class POSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
