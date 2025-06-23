using Kafo.DAL.Repository;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductManagementController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ProductManager _productManager;
		private readonly CategoryManager _categoryManager;

		public ProductManagementController(IUnitOfWork unitOfWork, ProductManager productManager, CategoryManager categoryManager)
		{
			_unitOfWork = unitOfWork;
			_productManager = productManager;
			_categoryManager = categoryManager;
		}

		// GET: ProductController

		public ActionResult Index()
		{
			//   var Products = productManager.GetAll(includeProperties: "Category");
			List<Category> categories = _categoryManager.GetAll();
			List<CategoryVM> categoryVMs = new List<CategoryVM>();
			foreach (Category category in categories)
			{
				CategoryVM categoryVM = new CategoryVM() { Description = category.Description, Id = category.Id, Name = category.Name };
				categoryVMs.Add(categoryVM);

			}
			ViewBag.Categories = categoryVMs;

			return View("Create");
		}
		public ActionResult Details(int? id)
		{
			if (id == null)
				return BadRequest();
			var Product = _productManager.Get(p => p.Id == id, "Category");

			return View("Details", Product);
		}

		public async Task<IActionResult> Upsert(ProductVM productVM, IFormFile? imageFile)
		{
			if (ModelState.IsValid)
			{
				Product product = new Product();
				product.ImageUrl = await UploadFile(imageFile, product.Name);
				product.Name = productVM.Name;
				product.Category = productVM.Category;
				product.CategoryId = productVM.CategoryId;
				product.AveragePurchasePrice = productVM.AveragePurchasePrice;
				product.SellingPrice = productVM.SellingPrice;
				product.IsActive = productVM.IsActive;


				if (productVM.Id == null)
				{
					_productManager.Add(product);

					TempData["Success"] = "تم إضافة المنتج بنجاح";
				}
				else
				{
					product = _productManager.Get(p => p.Id == productVM.Id);
					if (product == null)
					{
						return NotFound();
					}

					if (imageFile != null)
					{
						product.ImageUrl = await UploadFile(imageFile, productVM.Name);
					}
					product.Name = productVM.Name;
					product.Category = productVM.Category;
					product.CategoryId = productVM.CategoryId;
					product.AveragePurchasePrice = productVM.AveragePurchasePrice;
					product.SellingPrice = productVM.SellingPrice;
					product.IsActive = productVM.IsActive;
					_productManager.Update(product);
					TempData["Success"] = "تم تعديل المنتج بنجاح";
				}


				return RedirectToAction("Index", "Admin", new { area = "Admin" });
			}
			else
			{
				ViewBag.Categories = _categoryManager.GetAll();

				return View(productVM);
			}

		}
		public ActionResult Edit(int? id)
		{
			if (id == null)
				return BadRequest();
			var Categories = _categoryManager.GetAll();
			var product = _productManager.Get(p => p.Id == id, "Category");
			ViewBag.Categories = Categories;
			ProductVM productVM = new ProductVM()
			{
				Id = product.Id

				,
				CategoryId = product.CategoryId
				,
				ImageUrl = product.ImageUrl,
				IsActive = product.IsActive
				,
				Name = product.Name,
				SellingPrice = product.SellingPrice,
				AveragePurchasePrice = product.AveragePurchasePrice,
				StockQuantity = product.StockQuantity
			};
			return View(productVM);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				//  TempData["Error"] = "حدث خطأ";
				return Json(new { success = false, message = "المنتج غير موجود" });
			}

			string res = _productManager.Delete(id.Value);
			if (res == null)
			{
				return Json(new { success = true });
			}
			else
			{
				return Json(new { success = false, message = res });
			}

		}
		// POST: ProductController/Delete/5
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(int? id)
		//{
		//    if (id == null)
		//        return BadRequest();
		//    try
		//    {
		//        var p = _productManager.Get(p => p.Id == id);
		//        if (p != null)
		//        {
		//            _productManager.Remove(p);
		//        }
		//        return RedirectToAction("Index", "Admin", new { area = "Admin" });
		//    }
		//    catch
		//    {
		//        return StatusCode(500, "An error occurred while processing your request");
		//    }
		//}

		private async Task<string?> UploadFile(IFormFile? imageFile, string productName)
		{
			if (imageFile != null && imageFile.Length > 0)
			{
				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/products");
				Directory.CreateDirectory(uploadsFolder); // make sure folder exists
														  //uniqe => productName only
				var fileName = $"[{productName}] - {DateTime.Now.ToString("dd-MM-yyy-HH-mm-ss")}{Path.GetExtension(imageFile.FileName)}"; // safer filename
				var filePath = Path.Combine(uploadsFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				// Save image URL in your model
				return $"/images/Upload/products/{fileName}";
			}
			return null;
		}
	}
}
