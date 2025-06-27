using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;

namespace KAFO.BLL.Managers
{
	public class ReportManager
	{
		IUnitOfWork UOW = null;
		public ReportManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
		public double GetTotalProfit(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);


			var invoiceIds = UOW.Invoices.GetAll("Items.Product",filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).
				Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).ToList();
			var totalProfit = (double)invoiceIds.SelectMany(i => i.Items.Where(i => !i.IsDeleted)).Sum(item => (item.UnitSellingPrice - item.UnitPurchasingPrice) * item.Quantity);

		//	var totalProfit = (double)UOW.InvoiceItems.GetAll().Where(i => !i.IsDeleted && invoiceIds.Contains(i.Id)).Sum(i => (i.UnitSellingPrice - i.UnitPurchasingPrice) * i.Quantity);
			return totalProfit;
		}
		public double GetTodayTotalProfit()
		{
			DateTime today = DateTime.Today;
			return GetTotalProfit(today, today);
		}

		public double GetTotalSells(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			var totalSells = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Sum(i => i.TotalInvoice);
			return (double)totalSells;
		}
		public double GetTotalSellsToday()
		{
			DateTime today = DateTime.Today;
			return GetTotalSells(today, today);
		}
		public int GetTotalProductSold(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			List<InvoiceItem> totalItems = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate).SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
			var totalProducts = totalItems.Sum(i => i.Quantity);
			return (int)totalProducts;
		}
		public List<dynamic> GetMostProductSold(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);

			List<InvoiceItem> totalItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate).SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
			var products = totalItems.GroupBy(i => i.Product.Name).Select(group => new { productName = group.Key, totalQuantity = group.Sum(item => item.Quantity) });
			var topProducts = products.OrderByDescending(p => p.productName).Take(5).ToList<dynamic>();
			return topProducts;
		}
		public List<dynamic> GetLeastProductSold(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);

			List<InvoiceItem> totalItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate).SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
			var products = totalItems.GroupBy(i => i.Product.Name).Select(group => new { productName = group.Key, totalQuantity = group.Sum(item => item.Quantity) });
			var topProducts = products.OrderBy(p => p.productName).Take(5).ToList<dynamic>();
			return topProducts;
		}
		public int GetTotalProductSoldToday()
		{
			DateTime today = DateTime.Today;
			return GetTotalProductSold(today, today);
		}
		public dynamic GetMostProftableSeller(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);

			var totalInvoicesProfits = UOW.Invoices.GetAll("Items.Product,User", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).
				Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
				.Select(i => new { SellerName = i.User.Name, InvoiceProfit = i.Items.Sum(item => (item.UnitSellingPrice - item.UnitPurchasingPrice) * item.Quantity) })
				.ToList();
			var topSellers = totalInvoicesProfits.GroupBy(i => i.SellerName).Select(group => new { sellerName = group.Key, totalProfit = group.Sum(i => i.InvoiceProfit) }).OrderByDescending(i => i.totalProfit).Take(5).ToList();
			return topSellers;
		}
		public dynamic GetMostProftableProduct(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);

			var totalProductsProfits = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash).
				Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
				.SelectMany(i => i.Items.Where(item => !item.IsDeleted)).Select(item => new { ProductName = item.Product.Name, ProductProfit = (item.UnitSellingPrice - item.UnitPurchasingPrice) * item.Quantity });
			var topSellers = totalProductsProfits.GroupBy(i => i.ProductName).Select(group => new { productName = group.Key, totalProfit = group.Sum(i => i.ProductProfit) }).OrderByDescending(i => i.totalProfit).Take(5).ToList();
			return topSellers;
		}
		public int GetTotalSoldInvoices(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			var count = UOW.Invoices.GetAll(filter: i => (i.Type == InvoiceType.Cash || i.Type == InvoiceType.Credit) && !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Count();
			return count;
		}
		public double GetTotalPurchaseInvoicesAmount(DateTime startDate, DateTime endDate)
		{
			DateTime inclusiveEndDate = endDate.Date.AddDays(1);
			var total = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Purchasing && !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Sum(i => i.TotalInvoice);
			return (double)total;
		}
		public double GetTotalStockMoney()
		{
			var products = UOW.Products.GetAll(filter: p => !p.IsDeleted);
			var total = products.Sum(p => p.StockQuantity * p.SellingPrice);
			return (double)total;
		}
		public double GetTotalExpectedProfits()
		{
			var products = UOW.Products.GetAll(filter: p => !p.IsDeleted);
			var total = products.Sum(p => p.StockQuantity * (p.SellingPrice - p.AveragePurchasePrice));
			return (double)total;
		}
	}
}
