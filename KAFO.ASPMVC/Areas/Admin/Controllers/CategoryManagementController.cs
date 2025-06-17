using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.Domain.Products;
using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                TempData["Success"] = "there is erorr";
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {            
                return View("Index", categoryVM);
            }
            else
            {
                Category category = _categoryManager.Get(categoryVM.Id);
                if (category == null)
                {
                    category = new Category(name: categoryVM.Name, description: categoryVM.Description);

                    _categoryManager.Add(category);
                    TempData["Success"] = "تم إضافة الفئة بنجاح";
                }
                else
                {
                    _categoryManager.Update(category);
                    TempData["Success"] = "تم تعديل الفئة بنجاح";
                }
            }
            return RedirectToAction("Index", "Admin", new { area = "Admin" });

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Success"] = "حدث خطأ";
            }
            Category category = _categoryManager.Get(id.Value);
            //	_categoryManager.Delete(category);
            //TempData["Success"] = "تم حذف الفئة بنجاح";
            //return RedirectToAction("Index", "Admin", new { area = "Admin", page = 1 });
            return Json(new { success = true });
        }
    }
}

