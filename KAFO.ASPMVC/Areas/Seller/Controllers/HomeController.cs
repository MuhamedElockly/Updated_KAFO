using Kafo.DAL.Repository;
using KAFO.ASPMVC.Models;
using KAFO.BLL.Managers;
using KAFO.Domain.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "seller")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InvoiceManager _invoiceManager;

        public HomeController(IUnitOfWork unitOfWork, InvoiceManager invoiceManager)
        {
            _unitOfWork = unitOfWork;
            _invoiceManager = invoiceManager;
        }
        public IActionResult Index()
        {
            // Get current user ID from claims
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                TempData["Error"] = "?? ??? ?????? ??? ???? ???????? ??????";
                return RedirectToAction("Index", "Home");
            }

            // Get today's date
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // get terminations for today
            var Terminations = _unitOfWork.CreditTerminateInvoice.GetAll()
                .Where(t => t.CreatedAt >= today && t.CreatedAt < tomorrow);

            // get withdrawals for today
            var Withdrawals = _unitOfWork.CreditWithdrawInvoices.GetAll()
                .Where(w => w.CreatedAt >= today && w.CreatedAt < tomorrow);

            //get all invoices for charts (not filtered by today)
            var allInvoices = _invoiceManager.GetInvoicesByUser(userId);

            //get invoices for today only (for statistics cards)
            var todayInvoices = allInvoices.Where(i => i.CreatedAt >= today && i.CreatedAt < tomorrow).ToList();

            ViewBag.Invoices = allInvoices; // For charts
            ViewBag.TodayInvoices = todayInvoices; // For statistics cards

            // get products
            ViewBag.Products = _unitOfWork.Products.GetAll(filter: p => p.IsActive && p.StockQuantity < 20);

            // Calculate today's money
            var cashMoney = todayInvoices.Where(i => i.Type == InvoiceType.Cash).Where(t => t.CreatedAt >= today && t.CreatedAt < tomorrow).Sum(i => i.TotalInvoice);
            var returnMoney = todayInvoices.Where(i => i.Type == InvoiceType.CashReturn).Where(t => t.CreatedAt >= today && t.CreatedAt < tomorrow).Sum(i => i.TotalInvoice);
            var terminateMoney = Terminations.Sum(i => i.TotalInvoice);
            var withdrawMoney = Withdrawals.Sum(i => i.TotalInvoice);
            ViewBag.Money = cashMoney + terminateMoney - returnMoney - withdrawMoney;

            // Calculate daily sales for the last 7 days
            var dailySales = new List<object>();
            for (int i = 6 ; i >= 0 ; i--)
            {
                var date = today.AddDays(-i);
                var nextDate = date.AddDays(1);

                var dayInvoices = allInvoices.Where(i => i.CreatedAt >= date && i.CreatedAt < nextDate).ToList();

                var dayCash = dayInvoices.Where(i => i.Type == InvoiceType.Cash).Sum(i => i.TotalInvoice);
                var dayCredit = dayInvoices.Where(i => i.Type == InvoiceType.Credit).Sum(i => i.TotalInvoice);
                var dayCashReturn = dayInvoices.Where(i => i.Type == InvoiceType.CashReturn).Sum(i => i.TotalInvoice);
                var dayCreditReturn = dayInvoices.Where(i => i.Type == InvoiceType.CreditReturn).Sum(i => i.TotalInvoice);

                var dayTotal = dayCash + dayCredit - dayCashReturn - dayCreditReturn;

                dailySales.Add(new
                {
                    Date = date.ToString("dd/MM"),
                    Total = dayTotal
                });
            }

            ViewBag.DailySales = dailySales;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
