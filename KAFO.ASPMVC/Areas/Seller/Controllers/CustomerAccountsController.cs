using KAFO.BLL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "seller")]
    public class CustomerAccountsController : Controller
    {
        private readonly CreditCustomerManager _creditCustomerManager;

        public CustomerAccountsController(CreditCustomerManager creditCustomerManager)
        {
            _creditCustomerManager = creditCustomerManager;
        }

        // GET: CustomerAccounts
        public IActionResult Index()
        {
            var customers = _creditCustomerManager.GetAll()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Id)
                .ToList();
            
            return View(customers);
        }

        // GET: CustomerAccounts/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var customerAccount = _creditCustomerManager.Get(id.Value);
            if (customerAccount == null) return NotFound();

            return View(customerAccount);
        }

       

        // AJAX: CustomerAccounts/SettleAccountAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SettleAccountAjax(int customerId, decimal amount)
        {
            try
            {
                var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Json(new { success = false, message = "لم يتم العثور على معرف المستخدم الحالي" });
                }

                var result = _creditCustomerManager.SettleAccount(customerId, amount, userId);

                return Json(new { 
                    success = result.Success, 
                    message = result.Message,
                    customerId = result.CustomerId,
                    newTotalOwed = result.NewTotalOwed,
                    newTotalPaid = result.NewTotalPaid,
                    oldTotalOwed = result.OldTotalOwed,
                    oldTotalPaid = result.OldTotalPaid
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تصفية الحساب: " + ex.Message });
            }
        }
    }
}
