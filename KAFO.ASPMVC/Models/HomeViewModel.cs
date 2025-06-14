using KAFO.Domain.Products;
using KAFO.Domain.Users;
using KAFO.Domain.Products;
using KAFO.Domain.Users;

namespace Kafo.ASPMVC.Models
{
	public class HomeViewModel
	{
		public List<Product> Products { get; set; }= new List<Product>();
		public List<User> Sellers { get; set; } = new List<User>();
		public List<Category> Categories { get; set; } = new List<Category>();
		public List<KAFO.Domain.Users.User> Admins { get; set; } = new List<KAFO.Domain.Users.User>();
		public int TotalProducts { get; set; }
		public int TotalSellers { get; set; }
		public int TotalCategories { get; set; }
		public int TotalAdmins { get; set; }
		public decimal TotalSales { get; set; }
		public decimal TotalProfit { get; set; }
		public int TotalUsers { get; set; }
		public int PendingOrders { get; set; }
		public int TotalPayments { get; set; }
		public string MostSoldProduct { get; set; }
		public string MostProfitableSeller { get; set; }
		public string MostProfitableProduct { get; set; }
		public string ReportDate { get; set; }

		// Pagination properties
		public int CurrentProductPage { get; set; }
		public int TotalProductPages { get; set; }
		public int CurrentSellerPage { get; set; }
		public int TotalSellerPages { get; set; }
		public int CurrentCategoryPage { get; set; }
		public int TotalCategoryPages { get; set; }
		public int CurrentAdminPage { get; set; }
		public int TotalAdminPages { get; set; }

		// Today's statistics
		public decimal TodayProfit { get; set; }
		public decimal TodaySales { get; set; }
		public int TodayProductsSold { get; set; }

		// Pagination properties for Sellers
		public int SellerPageSize { get; set; }

		// Pagination properties for Categories
		public int CategoryPageSize { get; set; }

		// Pagination properties for Products
		public int ProductPageSize { get; set; }

		// Pagination properties for Admins
		public int AdminPageSize { get; set; }
	}
}