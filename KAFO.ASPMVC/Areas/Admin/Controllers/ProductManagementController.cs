using Kafo.DAL.Repository;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
		// GET: ProductController/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
				return BadRequest();
			var Product = _productManager.Get(p => p.Id == id, "Category");

			return View("~/Areas/Admin/Views/ProductManagement/Index.cshtml");
		}
		[HttpGet]
		// GET: ProductController/Create
		public ActionResult Create()
		{
			try
			{
				var Categories = _unitOfWork.Category.GetAll();
				ViewBag.Categories = Categories;
				return View();

			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while processing your request");
			}
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Product product, IFormFile imageFile)
		{
			try
			{
				if (ModelState.IsValid)
				{

					product.ImageUrl = await UploadFile(imageFile, product.Name);

					//BLL MAke LastPurchasing = AVGPurchasing
					_productManager.Add(product);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return Create();
				}
			}
			catch
			{
				return StatusCode(500, "An error occurred while processing your request");
			}
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

				if (productVM.Id == 0)
				{
					_productManager.Add(product);

					TempData["Success"] = "تم إضافة المنتج بنجاح";
				}
				else
				{
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
			var Categories = _unitOfWork.Category.GetAll();
			var Product = _productManager.Get(p => p.Id == id, "Category");
			ViewBag.Categories = Categories;
			return View(Product);
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, Product product, IFormFile? imageFile)
		{
			if (id == null)
				return BadRequest();
			try
			{
				if (ModelState.IsValid)
				{

					product.ImageUrl = await UploadFile(imageFile, product.Name);
					_productManager.AddORUpdate((int)id, product);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return Edit(id);
				}
			}
			catch
			{
				return StatusCode(500, "An error occurred while processing your request");
			}
		}

		// GET: ProductController/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return BadRequest();

			var Product = _productManager.Get(p => p.Id == id, "Category");

			return View(Product);
		}

		// POST: ProductController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int? id)
		{
			if (id == null)
				return BadRequest();
			try
			{
				var p = _productManager.Get(p => p.Id == id);
				if (p != null)
				{
					_productManager.Remove(p);
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return StatusCode(500, "An error occurred while processing your request");
			}
		}

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
