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


            var salesInvoices = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).ToList();


            var returnInvoices = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).ToList();

            var salesProfit = (double) salesInvoices.SelectMany(i => i.Items.Where(i => !i.IsDeleted))
                .Sum(item => ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity);


            var returnLoss = (double) returnInvoices.SelectMany(i => i.Items.Where(i => !i.IsDeleted))
                .Sum(item => ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity);


            var totalProfit = salesProfit - returnLoss;

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

            var salesInvoices = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate);
            var totalSales = salesInvoices.Sum(i => i.TotalInvoice);

            var returnInvoices = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate);
            var totalReturns = returnInvoices.Sum(i => i.TotalInvoice);

            var netSales = totalSales - totalReturns;

            return (double) netSales;
        }
        public double GetTotalSellsToday()
        {
            DateTime today = DateTime.Today;
            return GetTotalSells(today, today);
        }
        public int GetTotalProductSold(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);


            List<InvoiceItem> soldItems = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
            var totalSold = soldItems.Sum(i => i.Quantity);


            List<InvoiceItem> returnedItems = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();
            var totalReturned = returnedItems.Sum(i => i.Quantity);


            var netProductsSold = totalSold - totalReturned;

            return (int) netProductsSold;
        }
        public List<dynamic> GetMostProductSold(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);


            List<InvoiceItem> soldItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();


            List<InvoiceItem> returnedItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();


            var soldProducts = soldItems.GroupBy(i => i.Product.Name)
                .Select(group => new { ProductName = group.Key, SoldQuantity = group.Sum(item => item.Quantity) });

            var returnedProducts = returnedItems.GroupBy(i => i.Product.Name)
                .Select(group => new { ProductName = group.Key, ReturnedQuantity = group.Sum(item => item.Quantity) });


            var allProducts = soldProducts.Select(s => s.ProductName)
                .Union(returnedProducts.Select(r => r.ProductName))
                .Distinct();

            var netProducts = allProducts.Select(productName =>
            {
                var soldQty = soldProducts.FirstOrDefault(s => s.ProductName == productName)?.SoldQuantity ?? 0;
                var returnedQty = returnedProducts.FirstOrDefault(r => r.ProductName == productName)?.ReturnedQuantity ?? 0;
                return new { productName, totalQuantity = soldQty - returnedQty };
            });

            var topProducts = netProducts.OrderByDescending(p => p.totalQuantity).Take(5).ToList<dynamic>();
            return topProducts;
        }
        public List<dynamic> GetLeastProductSold(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);


            List<InvoiceItem> soldItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();

            List<InvoiceItem> returnedItems = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(i => !i.IsDeleted)).ToList();

            var soldProducts = soldItems.GroupBy(i => i.Product.Name)
                .Select(group => new { ProductName = group.Key, SoldQuantity = group.Sum(item => item.Quantity) });

            var returnedProducts = returnedItems.GroupBy(i => i.Product.Name)
                .Select(group => new { ProductName = group.Key, ReturnedQuantity = group.Sum(item => item.Quantity) });

            var allProducts = soldProducts.Select(s => s.ProductName)
                .Union(returnedProducts.Select(r => r.ProductName))
                .Distinct();

            var netProducts = allProducts.Select(productName =>
            {
                var soldQty = soldProducts.FirstOrDefault(s => s.ProductName == productName)?.SoldQuantity ?? 0;
                var returnedQty = returnedProducts.FirstOrDefault(r => r.ProductName == productName)?.ReturnedQuantity ?? 0;
                return new { productName, totalQuantity = soldQty - returnedQty };
            });

            var topProducts = netProducts.OrderBy(p => p.totalQuantity).Take(5).ToList<dynamic>();
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

            var salesInvoicesProfits = UOW.Invoices.GetAll("Items.Product,User", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
                .Select(i => new { SellerName = i.User.Name, InvoiceProfit = i.Items.Sum(item => ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity) })
                .ToList();

            var returnInvoicesLosses = UOW.Invoices.GetAll("Items.Product,User", filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
                .Select(i => new { SellerName = i.User.Name, InvoiceLoss = i.Items.Sum(item => ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity) })
                .ToList();

            var salesProfits = salesInvoicesProfits.GroupBy(i => i.SellerName)
                .Select(group => new { SellerName = group.Key, TotalProfit = group.Sum(i => i.InvoiceProfit) });

            var returnLosses = returnInvoicesLosses.GroupBy(i => i.SellerName)
                .Select(group => new { SellerName = group.Key, TotalLoss = group.Sum(i => i.InvoiceLoss) });
            var allSellers = salesProfits.Select(s => s.SellerName)
                .Union(returnLosses.Select(r => r.SellerName))
                .Distinct();

            var netSellerProfits = allSellers.Select(sellerName =>
            {
                var salesProfit = salesProfits.FirstOrDefault(s => s.SellerName == sellerName)?.TotalProfit ?? 0;
                var returnLoss = returnLosses.FirstOrDefault(r => r.SellerName == sellerName)?.TotalLoss ?? 0;
                return new { sellerName, totalProfit = salesProfit - returnLoss };
            });

            var topSellers = netSellerProfits.OrderByDescending(i => i.totalProfit).Take(5).ToList();
            return topSellers;
        }
        public dynamic GetMostProftableProduct(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);
            var salesProductsProfits = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.Credit || i.Type == InvoiceType.Cash)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(item => !item.IsDeleted))
                .Select(item => new { ProductName = item.Product.Name, ProductProfit = ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity });

            var returnProductsLosses = UOW.Invoices.GetAll("Items.Product", filter: i => i.Type == InvoiceType.CashReturn || i.Type == InvoiceType.CreditReturn)
                .Where(i => !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate)
                .SelectMany(i => i.Items.Where(item => !item.IsDeleted))
                .Select(item => new { ProductName = item.Product.Name, ProductLoss = ( item.UnitSellingPrice - item.UnitPurchasingPrice ) * item.Quantity });

            var salesProfits = salesProductsProfits.GroupBy(i => i.ProductName)
                .Select(group => new { ProductName = group.Key, TotalProfit = group.Sum(i => i.ProductProfit) });

            var returnLosses = returnProductsLosses.GroupBy(i => i.ProductName)
                .Select(group => new { ProductName = group.Key, TotalLoss = group.Sum(i => i.ProductLoss) });

            var allProducts = salesProfits.Select(s => s.ProductName)
                .Union(returnLosses.Select(r => r.ProductName))
                .Distinct();

            var netProductProfits = allProducts.Select(productName =>
            {
                var salesProfit = salesProfits.FirstOrDefault(s => s.ProductName == productName)?.TotalProfit ?? 0;
                var returnLoss = returnLosses.FirstOrDefault(r => r.ProductName == productName)?.TotalLoss ?? 0;
                return new { productName, totalProfit = salesProfit - returnLoss };
            });

            var topProducts = netProductProfits.OrderByDescending(i => i.totalProfit).Take(5).ToList();
            return topProducts;
        }
        public int GetTotalSoldInvoices(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);
            var count = UOW.Invoices.GetAll(filter: i => ( i.Type == InvoiceType.Cash || i.Type == InvoiceType.Credit ) && !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate).Count();
            return count;
        }
        public double GetTotalPurchaseInvoicesAmount(DateTime startDate, DateTime endDate)
        {
            DateTime inclusiveEndDate = endDate.Date.AddDays(1);

            var purchaseInvoices = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.Purchasing && !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate);
            var totalPurchases = purchaseInvoices.Sum(i => i.TotalInvoice);


            var returnPurchaseInvoices = UOW.Invoices.GetAll(filter: i => i.Type == InvoiceType.PurchasingReturn && !i.IsDeleted && i.CreatedAt >= startDate.Date && i.CreatedAt <= inclusiveEndDate);
            var totalPurchaseReturns = returnPurchaseInvoices.Sum(i => i.TotalInvoice);

            var netPurchaseAmount = totalPurchases - totalPurchaseReturns;

            return (double) netPurchaseAmount;
        }
        public double GetTotalStockMoney()
        {
            var products = UOW.Products.GetAll(filter: p => !p.IsDeleted);
            var total = products.Sum(p => p.StockQuantity * p.SellingPrice);
            return (double) total;
        }
        public double GetTotalExpectedProfits()
        {
            var products = UOW.Products.GetAll(filter: p => !p.IsDeleted);
            var total = products.Sum(p => p.StockQuantity * ( p.SellingPrice - p.AveragePurchasePrice ));
            return (double) total;
        }
    }
}