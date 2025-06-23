using Kafo.ASPMVC.Models;
using Kafo.DAL.Repository;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly CategoryManager _categoryManager;
		private readonly ReportManager _reportManager;
		private readonly ProductManager _productManager;
		private readonly UserManager _userManager;
		const int pageSize = 5;
		public AdminController(CategoryManager categoryManager, ReportManager invoiceManager, ProductManager productManager, UserManager userManager)
		{
			_categoryManager = categoryManager;
			_reportManager = invoiceManager;
			_productManager = productManager;
			_userManager = userManager;
		}

		public IActionResult Index(int sellerPage = 1, int categoryPage = 1, int productPage = 1, int adminPage = 1)
		{

			List<Product> products = _productManager.GetAll("Category").Take(pageSize).ToList();

			List<ProductVM> allProducts = new List<ProductVM>();
			foreach (Product product in products)
			{
				allProducts.Add(new ProductVM()
				{
					Id = product.Id,
					Name = product.Name,
					ImageUrl = product.ImageUrl,
					Category = product.Category,
					IsActive = product.IsActive
					,
					AveragePurchasePrice = product.AveragePurchasePrice,
					SellingPrice = product.SellingPrice,
					StockQuantity = product.StockQuantity
				});
			}
			int x = _categoryManager.GetAll().Count();

			ProductsTableVM productsTableVM = new ProductsTableVM()
			{
				Products = allProducts,
				CurrentProductPage = 1,
				TotalProductPages = (int)Math.Ceiling(_productManager.GetAll().Count() / (double)pageSize)
			};
			ViewData["Title"] = "لوحة التحكم";
			AdminHomeVM adminHomeVM = new AdminHomeVM { Products = productsTableVM, AdminName = "Muhamed Elockly", TotalProfitToday = _reportManager.GetTodayTotalProfit(), TotalProductSoldToday = _reportManager.GetTotalProductSoldToday(), TotalSellsToday = _reportManager.GetTotalSellsToday() };

			return View(adminHomeVM);
		}

		public IActionResult Reports()
		{
			var model = new HomeViewModel(); // Replace with your actual model
			return PartialView("_ReportsManagement", model);
		}
		public IActionResult Users(int? page)
		{
			List<User> users = _userManager.GetAll().Skip(pageSize * (page.Value - 1)).Take(pageSize).ToList();
			List<UserVM> allUsersVM = new List<UserVM>();
			foreach (var user in users)
			{
				UserVM userVM = new UserVM() { Name = user.Name, Id = user.Id, Email = user.Email, Phone = user.PhoneNumber, Role = user.Role };
				allUsersVM.Add(userVM);
			}

			UsersTableVM usersTableVM = new UsersTableVM();
			usersTableVM.CurrentUserPage = page ?? 1;
			usersTableVM.Users = allUsersVM;
			usersTableVM.TotalUsersPages = (int)Math.Ceiling(_userManager.GetAll().Count() / (double)pageSize);
			return PartialView("_UsersManagement", usersTableVM);
		}
		public IActionResult Categories(int? page)
		{
			if (page == null)
			{
				return BadRequest();
			}
			List<Category> categories = _categoryManager.GetAll().Skip(pageSize * (page.Value - 1)).Take(pageSize).ToList();

			List<CategoryVM> allCategories = new List<CategoryVM>();
			foreach (Category category in categories)
			{
				allCategories.Add(new CategoryVM() { Id = category.Id, Name = category.Name, Description = category.Description });
			}
			int x = _categoryManager.GetAll().Count();

			CategoriesTableVM categoriesTableVM = new CategoriesTableVM()
			{
				Categories = allCategories,
				CurrentCategoryPage = page ?? 1,
				TotalCategoriesPages = (int)Math.Ceiling(_categoryManager.GetAll().Count() / (double)pageSize)
			};

			return PartialView("_CategoriesManagement", categoriesTableVM);
		}
		public IActionResult Products(int? page)
		{
			if (page == null)
			{
				return BadRequest();

			}
			List<Product> products = _productManager.GetAll("Category").Skip(pageSize * (page.Value - 1)).Take(pageSize).ToList();

			List<ProductVM> allProducts = new List<ProductVM>();
			foreach (Product product in products)
			{
				allProducts.Add(new ProductVM()
				{
					Id = product.Id,
					Name = product.Name,
					ImageUrl = product.ImageUrl,
					Category = product.Category,
					IsActive = product.IsActive
					,
					AveragePurchasePrice = product.AveragePurchasePrice,
					SellingPrice = product.SellingPrice,
					StockQuantity = product.StockQuantity
				});
			}
			int x = _categoryManager.GetAll().Count();

			ProductsTableVM productsTableVM = new ProductsTableVM()
			{
				Products = allProducts,
				CurrentProductPage = page ?? 1,
				TotalProductPages = (int)Math.Ceiling(_productManager.GetAll().Count() / (double)pageSize)
			};

			return PartialView("_ProductsManagement", productsTableVM);
		}
	}
}
