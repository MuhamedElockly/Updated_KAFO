using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.Domain.Products;
using Kafo.DAL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categoryVM = new CategoryVM();
            // return PartialView("~/Areas/Admin/Views/Admin/_CategoryModal.cshtml", categoryVM);
            return View();
        }

      
    }
}

