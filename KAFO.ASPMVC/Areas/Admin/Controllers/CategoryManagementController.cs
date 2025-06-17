using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.Domain.Products;
using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryManagementController : Controller
	{
		private readonly CategoryManager _categoryManager;

		public CategoryManagementController(CategoryManager categoryManager)
		{
			_categoryManager = categoryManager;
		}

		public IActionResult Index()
		{
			var categoryVM = new CategoryVM();
			return View("~/Areas/Admin/Views/CategoryManagement/Index.cshtml");
		}
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return BadRequest();
			}

			Category category = _categoryManager.Get(id.Value); // Use .Value since id is nullable

			if (category == null)
			{
				return NotFound(); // Category not found
			}
			CategoryVM categoryVM = new CategoryVM() { Id = category.Id, Name = category.Name, Description = category.Description };

			return View("~/Areas/Admin/Views/CategoryManagement/Index.cshtml", categoryVM);
		}
		public IActionResult UpSert(CategoryVM categoryVM)
		{
			if (categoryVM == null)
			{
				return BadRequest();
			}
			Category category = _categoryManager.Get(categoryVM.Id);
			if (category == null)
			{
				category = new Category(name: categoryVM.Name, description: categoryVM.Description);

				_categoryManager.Add(category);
			}
			else
			{
				_categoryManager.Update(category);
			}
            TempData["Success"] = "Product added successfuly";
            return RedirectToAction("Index", "Admin", new { area = "Admin" });

		}
		[HttpPost]
		public IActionResult Create(CategoryVM categoryVM)
		{
			if (ModelState.IsValid)
			{
				var category = new Category(categoryVM.Name, categoryVM.Description);
				_categoryManager.Add(category);
				return RedirectToAction("Index", "Admin", new { area = "Admin" });
			}
			return View(categoryVM);
		}
	}
}

