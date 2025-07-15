using Kafo.DAL.Data;
using KAFO.Domain.Users;
using KAFO.Domain.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    public class CustomerAccountsController : Controller
    {
        // change to CustomerAccounts Manager
        private readonly AppDBContext _context;

        public CustomerAccountsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: CustomerAccounts
        public async Task<IActionResult> Index()
        {
            var customers = await _context.CustomerAccounts
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
            
            return View(customers);
        }

        // GET: CustomerAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        // GET: CustomerAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,TotalPaid,TotalOwed")] CustomerAccount customerAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccounts.FindAsync(id);
            if (customerAccount == null)
            {
                return NotFound();
            }
            return View(customerAccount);
        }

        // POST: CustomerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,TotalPaid,TotalOwed")] CustomerAccount customerAccount)
        {
            if (id != customerAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAccountExists(customerAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        // POST: CustomerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerAccount = await _context.CustomerAccounts.FindAsync(id);
            if (customerAccount != null)
            {
                _context.CustomerAccounts.Remove(customerAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerAccountExists(int id)
        {
            return _context.CustomerAccounts.Any(e => e.Id == id);
        }

        // POST: CustomerAccounts/SettleAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SettleAccount(int customerId, decimal amount, string? notes)
        {
            try
            {
                if (amount <= 0)
                {
                    TempData["Error"] = "يجب أن يكون المبلغ أكبر من صفر";
                    return RedirectToAction(nameof(Index));
                }

                var customer = await _context.CustomerAccounts.FindAsync(customerId);
                if (customer == null)
                {
                    TempData["Error"] = "العميل غير موجود";
                    return RedirectToAction(nameof(Index));
                }

                // Get current user
                var userName = User?.FindFirst(ClaimTypes.Name)?.Value;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
                if (currentUser == null)
                {
                    TempData["Error"] = "لم يتم العثور على المستخدم الحالي";
                    return RedirectToAction(nameof(Index));
                }

                // Create credit terminate invoice
                var creditTerminateInvoice = new KAFO.Domain.Invoices.CreditTerminateInvoice
                {
                    CreatedAt = DateTime.Now,
                    User = currentUser,
                    CustomerAccount = customer,
                    TotalInvoice = amount
                };

                _context.CreditTerminateInvoices.Add(creditTerminateInvoice);

                // Update customer balance
                customer.TotalPaid += amount;

                await _context.SaveChangesAsync();

                TempData["Success"] = $"تم تصفية حساب العميل {customer.CustomerName} بمبلغ {amount:C} بنجاح";
                if (!string.IsNullOrEmpty(notes))
                {
                    TempData["Info"] = $"ملاحظات: {notes}";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء تصفية الحساب: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
