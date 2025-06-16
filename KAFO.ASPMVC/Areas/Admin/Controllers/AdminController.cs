using Kafo.ASPMVC.Models;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
	{
		const int pageSize = 5;
		public IActionResult Index(int sellerPage = 1, int categoryPage = 1, int productPage = 1, int adminPage = 1)
		{
			
			// Sample data creation using constructors

			//		var allProducts = new List<Product>
			//{
			//	new Product("منتج 1", 80, 100, 50, category: allCategories[0]) { IsActive = true },
			//	new Product("منتج 2", 150, 200, 30, category: allCategories[0]) { IsActive = true },
			//	new Product("منتج 3", 120, 150, 20, category: allCategories[1]) { IsActive = false },
			//	new Product("منتج 4", 250, 300, 10, category: allCategories[1]) { IsActive = true },
			//	new Product("منتج 5", 200, 250, 15, category: allCategories[2]) { IsActive = true },
			//	new Product("منتج 6", 100, 120, 40, category: allCategories[2]) { IsActive = true },
			//	new Product("منتج 7", 140, 180, 25, category: allCategories[3]) { IsActive = false },
			//	new Product("منتج 8", 180, 220, 35, category: allCategories[3]) { IsActive = true },
			//	new Product("منتج 9", 60, 90, 60, category: allCategories[4]) { IsActive = true },
			//	new Product("منتج 10", 300, 350, 5, category: allCategories[4]) { IsActive = false }
			//};

			//		var allSellers = new List<User>
			//{
			//	new User("البائع 1", "seller1@example.com", "بائع", "ذكر"),
			//	new User("البائع 2", "seller2@example.com", "بائع", "ذكر"),
			//	new User("البائع 3", "seller3@example.com", "بائع", "ذكر"),
			//	new User("البائع 4", "seller4@example.com", "بائع", "ذكر"),
			//	new User("البائع 5", "seller5@example.com", "بائع", "ذكر"),
			//	new User("البائع 6", "seller6@example.com", "بائع", "ذكر")
			//};

			//		var allAdmins = new List<User>
			//{
			//	new KAFO.Domain.Users.User("المسؤول 1", "admin1@example.com", "بائع", "ذكر"),
			//	new KAFO.Domain.Users.User("المسؤول 2", "admin2@example.com", "بائع", "ذكر"),
			//	new KAFO.Domain.Users.User("المسؤول 3", "admin3@example.com", "بائع", "ذكر"),

			//};

			//		const int pageSize = 5;

			//		var pagedSellers = allSellers.Skip((sellerPage - 1) * pageSize).Take(pageSize).ToList();
			//		var pagedCategories = allCategories.Skip((categoryPage - 1) * pageSize).Take(pageSize).ToList();
			//		var pagedProducts = allProducts.Skip((productPage - 1) * pageSize).Take(pageSize).ToList();
			//		var pagedAdmins = allAdmins.Skip((adminPage - 1) * pageSize).Take(pageSize).ToList();

			//var viewModel = new HomeViewModel
			//{
			//	Products = pagedProducts,
			//	Sellers = pagedSellers,
			//	Categories = pagedCategories,
			//	Admins = pagedAdmins,
			//	TotalProducts = allProducts.Count,
			//	TotalSellers = allSellers.Count,
			//	TotalCategories = allCategories.Count,
			//	TotalAdmins = allAdmins.Count,
			//	TotalSales = 50000,
			//	TotalProfit = 15000,
			//	TotalUsers = 100,
			//	PendingOrders = 5,
			//	TotalPayments = 200,
			//	MostSoldProduct = "لابتوب ديل",
			//	MostProfitableSeller = "أحمد محمد",
			//	MostProfitableProduct = "هاتف سامسونج",
			//	ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
			//	TodayProfit = 2500,
			//	TodaySales = 5000,
			//	TodayProductsSold = 15,
			//	CurrentProductPage = productPage,
			//	TotalProductPages = (int)Math.Ceiling(allProducts.Count / (double)pageSize),
			//	CurrentSellerPage = sellerPage,
			//	TotalSellerPages = (int)Math.Ceiling(allSellers.Count / (double)pageSize),
			//	CurrentCategoryPage = categoryPage,
			//	TotalCategoryPages = (int)Math.Ceiling(allCategories.Count / (double)pageSize),
			//	CurrentAdminPage = adminPage,
			//	TotalAdminPages = (int)Math.Ceiling(allAdmins.Count / (double)pageSize),
			//	ProductPageSize = pageSize,
			//	SellerPageSize = pageSize,
			//	CategoryPageSize = pageSize,
			//	AdminPageSize = pageSize
			//};
			var allCategories = new List<Category>
			{
				new Category("فئة 1", "هذه هي الفئة الأولى."),
				new Category("فئة 2", "هذه هي الفئة الثانية."),
				new Category("فئة 3", "هذه هي الفئة الثالثة."),
				new Category("فئة 4", "هذه هي الفئة الرابعة."),
				new Category("فئة 5", "هذه هي الفئة الخامسة.")
			};

			var allProducts = new List<ProductVM>
			{
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=56,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[3],ImageUrl="New Image Url", Id=1,Name="منتج رقم 3",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 5",SellingPrice=43,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[2],ImageUrl="New Image Url", Id=1,Name="منتج رقم11",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[4],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=89,StockQuantity=30},
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[0],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=22,StockQuantity=30},

			};


			ProductsTableVM productsTableVM = new ProductsTableVM()
			{
				CurrentProductPage = 1,
				Products = allProducts,
				TotalProductPages = (int)Math.Ceiling(allProducts.Count / (double)pageSize)
			};

			ViewData["Title"] = "لوحة التحكم";
			AdminHomeVM adminHomeVM = new AdminHomeVM { Products = productsTableVM, AdminName = "Muhamed Elockly", TotalProfitToday = 102.5, TotalProductSoldToday = 100, TotalSellsToday = 506.5 };

			return View(adminHomeVM);
		}

		public IActionResult Reports()
		{
			var model = new HomeViewModel(); // Replace with your actual model
			return PartialView("_ReportsManagement", model);
		}
		public IActionResult Users(int? page )
		{

			List<UserVM> allSellers = new List<UserVM>()
			{
				new UserVM() {Id = 1,Name="محمد محمود ",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Seller"},
				new UserVM() {Id = 1,Name="محمد محمود ",Phone="01029092093",Role="Admin"},
				new UserVM() {Id = 1,Name="احمد خالد",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Admin"},
				new UserVM() {Id = 1,Name="محمد محمود ",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Seller"},
				new UserVM() {Id = 2,Name="علي",Phone="01029092093",Role="Admin"},
				new UserVM() {Id = 4,Name="محمد محمود ",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Admin"},
				new UserVM() {Id = 5,Name="محمد محمود ",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Seller"},
				new UserVM() {Id = 1,Name="محمد محمود ",Phone="01029092093",Email="elocklymuhamed@gmail.com",Role="Admin"},

			};

			UsersTableVM sellersTableVM = new UsersTableVM();
			sellersTableVM.CurrentUserPage = page ?? 1;
			sellersTableVM.Users = allSellers;
			sellersTableVM.TotalUsersPages = (int)Math.Ceiling(allSellers.Count / (double)pageSize);

			return PartialView("_UsersManagement", sellersTableVM);
		}
		public IActionResult Categories(int? page)
		{

			List<CategoryVM> allCategories = new List<CategoryVM>()
			{
				new CategoryVM(){Id=1,Name="شيبسي",Description="منتج شيبس"},
				new CategoryVM(){Id=2,Name="شيبسي",Description="منتج شيبس"},
				new CategoryVM(){Id=4,Name="مشروبات",Description="منتج شيبس"},
				new CategoryVM(){Id=5,Name="شيبسي",Description="منتج شيبس"},
				new CategoryVM(){Id=6,Name="شيبسي",Description="منتج شيبس"},
				new CategoryVM(){Id=8,Name="شيبسي",Description="منتج شيبس"},
				new CategoryVM(){Id=4,Name="شيبسي",Description="منتج شيبس"},
			};

			CategoriesTableVM categoriesTableVM = new CategoriesTableVM()
			{
				Categories = allCategories,
				CurrentCategoryPage = page ?? 1,
				TotalCategoriesPages = (int)Math.Ceiling(allCategories.Count / (double)pageSize)
			};

			return PartialView("_CategoriesManagement", categoriesTableVM);
		}
		public IActionResult Products(int productPage = 1)
		{
			var allCategories = new List<Category>
			{
				new Category("فئة 1", "هذه هي الفئة الأولى."),
				new Category("فئة 2", "هذه هي الفئة الثانية."),
				new Category("فئة 3", "هذه هي الفئة الثالثة."),
				new Category("فئة 4", "هذه هي الفئة الرابعة."),
				new Category("فئة 5", "هذه هي الفئة الخامسة.")
			};

			var allProducts = new List<ProductVM>
			{
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=56,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[3],ImageUrl="New Image Url", Id=1,Name="منتج رقم 3",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 5",SellingPrice=43,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[2],ImageUrl="New Image Url", Id=1,Name="منتج رقم11",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[4],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=89,StockQuantity=30},
				new ProductVM() { IsActive = false,Category=allCategories[1],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=22,StockQuantity=30},
				new ProductVM() { IsActive = true,Category=allCategories[0],ImageUrl="New Image Url", Id=1,Name="منتج رقم 1",SellingPrice=22,StockQuantity=30},

			};
			int pageSize = 5;

			ProductsTableVM productsTableVM = new ProductsTableVM()
			{
				CurrentProductPage = productPage,
				Products = allProducts,
				TotalProductPages = (int)Math.Ceiling(allProducts.Count / (double)pageSize)
			};


			return PartialView("_ProductsManagement", productsTableVM);
		}
	}
}
