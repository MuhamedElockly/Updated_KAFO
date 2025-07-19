using Kafo.ASPMVC.Models;
using Kafo.DAL.Repository;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Invoices;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using KAFO.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly CategoryManager _categoryManager;
		private readonly ReportManager _reportManager;
		private readonly InventoryManager _inventoryManager;
		private readonly ProductManager _productManager;
		private readonly UserManager _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly CreditCustomerManager _creditCustomerManager;
		private readonly InvoiceManager _invoiceManager;
		const int pageSize = 5;
		public AdminController(CategoryManager categoryManager, ReportManager invoiceManager, InventoryManager inventoryManager, ProductManager productManager, UserManager userManager, IUnitOfWork unitOfWork, CreditCustomerManager creditCustomerManager, InvoiceManager invoiceManager2)
		{
			_categoryManager = categoryManager;
			_reportManager = invoiceManager;
			_productManager = productManager;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_inventoryManager = inventoryManager;
			_creditCustomerManager = creditCustomerManager;
			_invoiceManager = invoiceManager2;
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
			AdminHomeVM adminHomeVM = new AdminHomeVM { Products = productsTableVM, AdminName = User.Identity.Name, TotalProfitToday = _reportManager.GetTodayTotalProfit(), TotalProductSoldToday = _reportManager.GetTotalProductSoldToday(), TotalSellsToday = _reportManager.GetTotalSellsToday() };

			return View(adminHomeVM);
		}

		public IActionResult Reports()
		{
			var model = new HomeViewModel(); // Replace with your actual model
			return PartialView("_ReportsManagement", model);
		}

		public IActionResult Profile()
		{
			return RedirectToAction("Index", "Profile", new { area = "Admin" });
		}

		public IActionResult Users(int? page)
		{
			if (page == null)
			{
				return BadRequest();
			}

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

		public IActionResult Invoices(string invoiceType = "sell", DateTime? startDate = null, DateTime? endDate = null, int? page = 1)
		{
			var invoices = new List<InvoiceVM>();
			var query = _unitOfWork.Invoices.GetAll("User,Items");

			// Only process invoices if dates are provided
			if (startDate.HasValue && endDate.HasValue)
			{
				// Filter by date range if provided
				var inclusiveEndDate = endDate.Value.Date.AddDays(1);
				query = query.Where(i => i.CreatedAt >= startDate.Value.Date && i.CreatedAt <= inclusiveEndDate);

				bool hasRealInvoices = false;

				if (invoiceType == "sell")
				{
					var cashInvoices = query.Where(i => i.Type == InvoiceType.Cash);
					var creditInvoices = query.Where(i => i.Type == InvoiceType.Credit);

					cashInvoices = cashInvoices.Where(i => i.CreatedAt >= startDate.Value.Date && i.CreatedAt <= inclusiveEndDate);
					creditInvoices = creditInvoices.Where(i => i.CreatedAt >= startDate.Value.Date && i.CreatedAt <= inclusiveEndDate);

					hasRealInvoices = cashInvoices.Any() || creditInvoices.Any();

					foreach (var invoice in cashInvoices)
					{
						invoices.Add(new InvoiceVM
						{
							Id = invoice.Id,
							CreatedAt = invoice.CreatedAt,
							UserName = invoice.User?.Name ?? "غير محدد",
							TotalInvoice = invoice.TotalInvoice,
							InvoiceType = "نقدي",
							CustomerName = "-",
							ItemsCount = invoice.Items?.Count ?? 0,
							ImageUrl = invoice.ImageUrl
						});
					}

					foreach (var invoice in creditInvoices)
					{
						invoices.Add(new InvoiceVM
						{
							Id = invoice.Id,
							CreatedAt = invoice.CreatedAt,
							UserName = invoice.User?.Name ?? "غير محدد",
							TotalInvoice = invoice.TotalInvoice,
							InvoiceType = "آجل",
							CustomerName = invoice.CustomerAccount?.CustomerName ?? "غير محدد",
							ItemsCount = invoice.Items?.Count ?? 0,
							ImageUrl = invoice.ImageUrl
						});
					}
				}
				else if (invoiceType == "purchase")
				{
					var purchasingInvoices = query.Where(i => i.Type == InvoiceType.Purchasing);

					purchasingInvoices = purchasingInvoices.Where(i => i.CreatedAt >= startDate.Value.Date && i.CreatedAt <= inclusiveEndDate);

					hasRealInvoices = purchasingInvoices.Any();

					foreach (var invoice in purchasingInvoices)
					{
						invoices.Add(new InvoiceVM
						{
							Id = invoice.Id,
							CreatedAt = invoice.CreatedAt,
							UserName = invoice.User?.Name ?? "غير محدد",
							TotalInvoice = invoice.TotalInvoice,
							InvoiceType = "شراء",
							CustomerName = "-",
							ItemsCount = invoice.Items?.Count ?? 0,
							ImageUrl = invoice.ImageUrl
						});
					}
				}

				// If no real invoices, add mock data for testing
				if (!hasRealInvoices)
				{
					for (int i = 1; i <= 7; i++)
					{
						invoices.Add(new InvoiceVM
						{
							Id = 1000 + i,
							CreatedAt = DateTime.Now.AddDays(-i),
							UserName = "مسؤول " + i,
							TotalInvoice = 100 * i + (invoiceType == "purchase" ? 50 : 0),
							InvoiceType = invoiceType == "sell" ? (i % 2 == 0 ? "نقدي" : "آجل") : "شراء",
							CustomerName = invoiceType == "sell" ? (i % 2 == 0 ? "-" : $"عميل {i}") : "-",
							ItemsCount = 2 + (i % 3),
							ImageUrl = null
						});
					}
				}
			}

			// Apply pagination only if we have invoices
			var totalInvoices = invoices.Count;
			var currentPage = page ?? 1;
			var pagedInvoices = invoices
				.OrderByDescending(i => i.CreatedAt)
				.Skip(pageSize * (currentPage - 1))
				.Take(pageSize)
				.ToList();

			var invoicesTableVM = new InvoicesTableVM
			{
				Invoices = pagedInvoices,
				CurrentInvoicePage = currentPage,
				TotalInvoicesPages = (int)Math.Ceiling(totalInvoices / (double)pageSize),
				InvoiceType = invoiceType
			};

			return PartialView("_InvoicesManagement", invoicesTableVM);
		}

		public IActionResult GetInvoices(string invoiceType = "sell", DateTime? startDate = null, DateTime? endDate = null)
		{
			// Use the new InvoiceManager to get invoices
			var invoices = _invoiceManager.GetInvoicesForAdmin(invoiceType, startDate, endDate);
			
			// Return JSON result
			return Json(invoices);
		}

		public IActionResult DailyInventory()
		{
			// Get seller data directly as SellerInventoryVM list from BLL
			var sellersInventory = _inventoryManager.GetSellerInvetoryToday();
			
			// Get product data directly as ProductRemainVM list from BLL
			var productsRemain = _inventoryManager.GetProductInventoryToday();

			// Create the main view model
			var dailyInventoryVM = new DailyInventoryVM
			{
				SellersInventory = sellersInventory,
				ProductsRemain = productsRemain
			};

			return PartialView("_DailyInventory", dailyInventoryVM);
		}

		public IActionResult CreditCustomerManagement(int? page)
		{
			int pageSize = 5;
			int currentPage = page ?? 1;
			// Use real data from the manager
			var allCustomers = _creditCustomerManager.GetAll();
			var pagedCustomers = allCustomers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

			var vm = new KAFO.ASPMVC.Areas.Admin.ViewModels.CreditCustomersTableVM
			{
				CreditCustomers = pagedCustomers.Select(c => new KAFO.ASPMVC.Areas.Admin.ViewModels.CreditCustomerDisplayVM
				{
					Id = c.Id,
					Name = c.CustomerName,
					Phone = c.PhoneNumber ?? "", // Use real phone if available
					Email = c.Email, // Use real email if available
					Gender = c.Gender ?? "", // Use real gender if available
					Balance = c.Balance,
					Credit = c.TotalOwed
				}).ToList(),
				CurrentPage = currentPage,
				TotalPages = (int)System.Math.Ceiling(allCustomers.Count / (double)pageSize)
			};

			return PartialView("_CreditCustomersManagement", vm);
		}
	}
}