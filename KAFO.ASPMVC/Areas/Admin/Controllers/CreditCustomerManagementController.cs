using KAFO.ASPMVC.Areas.Admin.ViewModels;
using KAFO.BLL.Managers;
using KAFO.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Kafo.ASPMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CreditCustomerManagementController : Controller
    {
        private readonly CreditCustomerManager _creditCustomerManager;
        private const int pageSize = 5;

        public CreditCustomerManagementController(CreditCustomerManager creditCustomerManager)
        {
            _creditCustomerManager = creditCustomerManager;
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

        public IActionResult Create()
        {
            try
            {
                return View(new KAFO.ASPMVC.Areas.Admin.ViewModels.CreditCustomerAddVM());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(KAFO.ASPMVC.Areas.Admin.ViewModels.CreditCustomerAddVM model)
        {
            try
            {
                // Simulate add (no DB)
                TempData["Success"] = "تم إضافة العميل بنجاح!";
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                // TEMP: Use the same temp list
                var allCustomers = new List<CustomerAccount>
                {
                    new CustomerAccount { Id = 1, CustomerName = "أحمد علي", TotalPaid = 1000, TotalOwed = 500 },
                    new CustomerAccount { Id = 2, CustomerName = "سارة محمد", TotalPaid = 2000, TotalOwed = 1500 },
                    new CustomerAccount { Id = 3, CustomerName = "محمد حسن", TotalPaid = 500, TotalOwed = 700 }
                };
                var customer = allCustomers.FirstOrDefault(c => c.Id == id);
                if (customer == null) return Json(new { success = false, message = "العميل غير موجود" });
                var vm = new KAFO.ASPMVC.Areas.Admin.ViewModels.CreditCustomerDisplayVM
                {
                    Id = customer.Id,
                    Name = customer.CustomerName,
                    Phone = "01000000000", // Placeholder
                    Email = null, // Placeholder
                    Gender = "ذكر", // Placeholder
                    Balance = customer.Balance,
                    Credit = customer.TotalOwed
                };
                return Json(new { success = true, data = vm });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ غير متوقع: " + ex.Message });
            }
        }

        public IActionResult ViewAccount(int id)
        {
            try
            {
                var customer = _creditCustomerManager.GetWithInvoicesData(id);
                if (customer == null)
                {
                    TempData["Error"] = "العميل غير موجود";
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                var transactions = customer.Transactions?.Select(t => new CreditCustomerTransactionVM
                {
                    SellerName = t.SellerName,
                    Time = t.Time,
                    DepositMoney = t.DepositMoney
                }).ToList() ?? new List<CreditCustomerTransactionVM>();
                var vm = new CreditCustomerAccountVM
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Gender = customer.Gender,
                    Balance = customer.Balance,
                    Credit = customer.Credit,
                    Transactions = transactions,
                    CurrentPage = 1,
                    TotalPages = 1 // Client-side pagination
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

        }

        [HttpPost]
        public IActionResult UpSert(CreditCustomerVM model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return View("Index", model);

                //if (model.Id == 0)
                //{
                //    var customer = new CustomerAccount
                //    {
                //        Name = model.Name,
                //        Email = model.Email,
                //        PhoneNumber = model.Phone,
                //        Balance = model.Balance ?? 0,
                //        Credit = model.Credit ?? 0
                //    };
                //    _creditCustomerManager.Add(customer);
                //}
                //else
                //{
                //    var customer = _creditCustomerManager.Get(model.Id);
                //    if (customer == null) return NotFound();
                //    customer.Name = model.Name;
                //    customer.Email = model.Email;
                //    customer.PhoneNumber = model.Phone;
                //    customer.Balance = model.Balance ?? 0;
                //    customer.Credit = model.Credit ?? 0;
                //    _creditCustomerManager.Update(customer);
                //}
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _creditCustomerManager.Delete(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ غير متوقع: " + ex.Message });
            }
        }

        //public IActionResult Edit(int id)
        //{
        //    //var customer = _creditCustomerManager.Get(id);
        //    //if (customer == null) return NotFound();
        //    //var vm = new CreditCustomerVM
        //    //{
        //    //    Id = customer.Id,
        //    //    Name = customer.Name,
        //    //    Email = customer.Email,
        //    //    Phone = customer.PhoneNumber,
        //    //    Balance = customer.Balance,
        //    //    Credit = customer.Credit
        //    //};
        //    return View("Index", vm);
        //}
    }
} 