using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "seller")]
    public class POSController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InvoicesManager _invoicesManager;

        public POSController(IUnitOfWork unitOfWork, InvoicesManager invoicesManager)
        {
            _unitOfWork = unitOfWork;
            _invoicesManager = invoicesManager;
        }

        public IActionResult Index()
        {
            ViewBag.Products = _unitOfWork.Products.GetAll("Category", p => p.IsActive);
            ViewBag.Cats = _unitOfWork.Categories.GetAll();
            ViewBag.Customers = _unitOfWork.CustomerAccounts.GetAll();

            return View();
        }
    }
}
