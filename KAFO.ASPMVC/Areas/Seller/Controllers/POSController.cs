using Kafo.DAL.Repository;
using KAFO.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    public class POSController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public POSController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.Products = _unitOfWork.Product.GetAll("Category", p => p.IsActive);
            ViewBag.Cats = _unitOfWork.Category.GetAll();
            //ViewBag.Customers = _unitOfWork.CustomerAccount.GetAll();

            return View();
        }
        public IActionResult Create(InvoiceViewModel invoice)
        {
            var Products = _unitOfWork.Product.GetAll("Category", p => p.IsActive);
            ViewBag.Products = Products;
            var Cats = _unitOfWork.Category.GetAll();
            ViewBag.Cats = Cats;
            var Customers = _unitOfWork.CustomerAccount.GetAll();
            ViewBag.Customers = Customers;
            if (ModelState.IsValid)
            {
                invoice.CreatedAt = DateTime.Now;
                foreach (var item in invoice.Items)
                {

                }

                //if (invoice.Type == InvoiceType.Cash)
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
