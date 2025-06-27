using Kafo.ASPMVC.Models;
using Kafo.DAL.Repository;
using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Invoices;
using KAFO.Domain.Products;
using KAFO.Domain.Users;
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
        private readonly ProductManager _productManager;
        private readonly UserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        const int pageSize = 5;
        public AdminController(CategoryManager categoryManager, ReportManager invoiceManager, ProductManager productManager, UserManager userManager, IUnitOfWork unitOfWork)
        {
            _categoryManager = categoryManager;
            _reportManager = invoiceManager;
            _productManager = productManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
                TotalProductPages = (int) Math.Ceiling(_productManager.GetAll().Count() / (double) pageSize)
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
        public IActionResult Users(int? page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            List<User> users = _userManager.GetAll().Skip(pageSize * ( page.Value - 1 )).Take(pageSize).ToList();
            List<UserVM> allUsersVM = new List<UserVM>();
            foreach (var user in users)
            {
                UserVM userVM = new UserVM() { Name = user.Name, Id = user.Id, Email = user.Email, Phone = user.PhoneNumber, Role = user.Role };
                allUsersVM.Add(userVM);
            }

            UsersTableVM usersTableVM = new UsersTableVM();
            usersTableVM.CurrentUserPage = page ?? 1;
            usersTableVM.Users = allUsersVM;
            usersTableVM.TotalUsersPages = (int) Math.Ceiling(_userManager.GetAll().Count() / (double) pageSize);
            return PartialView("_UsersManagement", usersTableVM);
        }
        public IActionResult Categories(int? page)
        {
            if (page == null)
            {
                return BadRequest();
            }
            List<Category> categories = _categoryManager.GetAll().Skip(pageSize * ( page.Value - 1 )).Take(pageSize).ToList();

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
                TotalCategoriesPages = (int) Math.Ceiling(_categoryManager.GetAll().Count() / (double) pageSize)
            };

            return PartialView("_CategoriesManagement", categoriesTableVM);
        }
        public IActionResult Products(int? page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            List<Product> products = _productManager.GetAll("Category").Skip(pageSize * ( page.Value - 1 )).Take(pageSize).ToList();

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
                TotalProductPages = (int) Math.Ceiling(_productManager.GetAll().Count() / (double) pageSize)
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
                            ItemsCount = invoice.Items?.Count ?? 0
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
                            ItemsCount = invoice.Items?.Count ?? 0
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
                            ItemsCount = invoice.Items?.Count ?? 0
                        });
                    }
                }

                // If no real invoices, add mock data for testing
                if (!hasRealInvoices)
                {
                    for (int i = 1 ; i <= 7 ; i++)
                    {
                        invoices.Add(new InvoiceVM
                        {
                            Id = 1000 + i,
                            CreatedAt = DateTime.Now.AddDays(-i),
                            UserName = "مسؤول " + i,
                            TotalInvoice = 100 * i + ( invoiceType == "purchase" ? 50 : 0 ),
                            InvoiceType = invoiceType == "sell" ? ( i % 2 == 0 ? "نقدي" : "آجل" ) : "شراء",
                            CustomerName = invoiceType == "sell" ? ( i % 2 == 0 ? "-" : $"عميل {i}" ) : "-",
                            ItemsCount = 2 + ( i % 3 )
                        });
                    }
                }
            }

            // Apply pagination only if we have invoices
            var totalInvoices = invoices.Count;
            var currentPage = page ?? 1;
            var pagedInvoices = invoices
                .OrderByDescending(i => i.CreatedAt)
                .Skip(pageSize * ( currentPage - 1 ))
                .Take(pageSize)
                .ToList();

            var invoicesTableVM = new InvoicesTableVM
            {
                Invoices = pagedInvoices,
                CurrentInvoicePage = currentPage,
                TotalInvoicesPages = (int) Math.Ceiling(totalInvoices / (double) pageSize),
                InvoiceType = invoiceType
            };

            return PartialView("_InvoicesManagement", invoicesTableVM);
        }

        public IActionResult DailyInventory()
        {
            // Temporary test data for sellers inventory
            var sellersInventory = new[]
            {
                new { sellerName = "أحمد علي", phone = "01012345678", totalCashPayment = 1500, totalCreditPayment = 2000, totalRefundCredit = 300, totalSupplyMoney = 3200 },
                new { sellerName = "محمد حسن", phone = "01087654321", totalCashPayment = 1200, totalCreditPayment = 1800, totalRefundCredit = 200, totalSupplyMoney = 2800 },
                new { sellerName = "سالم محمود", phone = "01011112222", totalCashPayment = 1100, totalCreditPayment = 1700, totalRefundCredit = 100, totalSupplyMoney = 2500 },
                new { sellerName = "خالد يوسف", phone = "01033334444", totalCashPayment = 1300, totalCreditPayment = 1600, totalRefundCredit = 150, totalSupplyMoney = 2600 },
                new { sellerName = "محمود سعيد", phone = "01055556666", totalCashPayment = 1400, totalCreditPayment = 2100, totalRefundCredit = 250, totalSupplyMoney = 3200 },
                new { sellerName = "علي إبراهيم", phone = "01077778888", totalCashPayment = 1250, totalCreditPayment = 1950, totalRefundCredit = 180, totalSupplyMoney = 3100 },
                new { sellerName = "حسن فؤاد", phone = "01099990000", totalCashPayment = 1350, totalCreditPayment = 1850, totalRefundCredit = 220, totalSupplyMoney = 3200 },
                new { sellerName = "طارق عبد الله", phone = "01112345678", totalCashPayment = 1500, totalCreditPayment = 2000, totalRefundCredit = 300, totalSupplyMoney = 3200 },
                new { sellerName = "سعيد أحمد", phone = "01187654321", totalCashPayment = 1200, totalCreditPayment = 1800, totalRefundCredit = 200, totalSupplyMoney = 2800 },
                new { sellerName = "يوسف سالم", phone = "01111112222", totalCashPayment = 1100, totalCreditPayment = 1700, totalRefundCredit = 100, totalSupplyMoney = 2500 },
                new { sellerName = "إبراهيم خالد", phone = "01133334444", totalCashPayment = 1300, totalCreditPayment = 1600, totalRefundCredit = 150, totalSupplyMoney = 2600 },
                new { sellerName = "سعيد محمود", phone = "01155556666", totalCashPayment = 1400, totalCreditPayment = 2100, totalRefundCredit = 250, totalSupplyMoney = 3200 },
                new { sellerName = "فؤاد علي", phone = "01177778888", totalCashPayment = 1250, totalCreditPayment = 1950, totalRefundCredit = 180, totalSupplyMoney = 3100 },
                new { sellerName = "عبد الله حسن", phone = "01199990000", totalCashPayment = 1350, totalCreditPayment = 1850, totalRefundCredit = 220, totalSupplyMoney = 3200 },
                new { sellerName = "مروان طارق", phone = "01212345678", totalCashPayment = 1500, totalCreditPayment = 2000, totalRefundCredit = 300, totalSupplyMoney = 3200 }
            };
            // Temporary test data for products remain
            var productsRemain = new[]
            {
                new { productImage = "/images/Upload/products/sample1.jpg", productName = "منتج 1", remainBox = 10, remainPlus = 5, totalRemain = 105 },
                new { productImage = "/images/Upload/products/sample2.jpg", productName = "منتج 2", remainBox = 7, remainPlus = 2, totalRemain = 72 },
                new { productImage = "/images/Upload/products/sample3.jpg", productName = "منتج 3", remainBox = 12, remainPlus = 6, totalRemain = 126 },
                new { productImage = "/images/Upload/products/sample4.jpg", productName = "منتج 4", remainBox = 8, remainPlus = 3, totalRemain = 83 },
                new { productImage = "/images/Upload/products/sample5.jpg", productName = "منتج 5", remainBox = 15, remainPlus = 7, totalRemain = 157 },
                new { productImage = "/images/Upload/products/sample6.jpg", productName = "منتج 6", remainBox = 9, remainPlus = 4, totalRemain = 94 },
                new { productImage = "/images/Upload/products/sample7.jpg", productName = "منتج 7", remainBox = 11, remainPlus = 5, totalRemain = 115 },
                new { productImage = "/images/Upload/products/sample8.jpg", productName = "منتج 8", remainBox = 13, remainPlus = 6, totalRemain = 136 },
                new { productImage = "/images/Upload/products/sample9.jpg", productName = "منتج 9", remainBox = 6, remainPlus = 2, totalRemain = 62 },
                new { productImage = "/images/Upload/products/sample10.jpg", productName = "منتج 10", remainBox = 14, remainPlus = 7, totalRemain = 147 },
                new { productImage = "/images/Upload/products/sample11.jpg", productName = "منتج 11", remainBox = 10, remainPlus = 5, totalRemain = 105 },
                new { productImage = "/images/Upload/products/sample12.jpg", productName = "منتج 12", remainBox = 7, remainPlus = 2, totalRemain = 72 },
                new { productImage = "/images/Upload/products/sample13.jpg", productName = "منتج 13", remainBox = 12, remainPlus = 6, totalRemain = 126 },
                new { productImage = "/images/Upload/products/sample14.jpg", productName = "منتج 14", remainBox = 8, remainPlus = 3, totalRemain = 83 },
                new { productImage = "/images/Upload/products/sample15.jpg", productName = "منتج 15", remainBox = 15, remainPlus = 7, totalRemain = 157 }
            };
            ViewBag.SellersInventory = sellersInventory;
            ViewBag.ProductsRemain = productsRemain;
            return PartialView("_DailyInventory");
        }
    }
}