using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using KAFO.Domain.Invoices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KAFO.ASPMVC.Areas.Seller.Controllers
{
    [Area("seller")]
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InvoicesManager _invoicesManager;

        public InvoiceController(IUnitOfWork unitOfWork, InvoicesManager invoicesManager)
        {
            _unitOfWork = unitOfWork;
            _invoicesManager = invoicesManager;
        }
        // GET: InvoiceController
        public IActionResult Index()
        {
            var invoices = _invoicesManager.GetCompleteAll();
            return View(invoices);
        }

        // GET: InvoiceController/Details/5
        public IActionResult Details(int id)
        {
            var invoice = _invoicesManager.GetComplete(id);
            if (invoice == null)
                return NotFound();
            return View(invoice);
        }

        // GET: InvoiceController/Create
        public IActionResult Create()
        {
            ViewBag.Products = _unitOfWork.Products.GetAll("Category", p => p.IsActive);
            return View();
        }

        // POST: InvoiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Invoice invoice, string dist = "")
        {
            int checkResult = CheckInvoice(invoice);
            if (checkResult < 0)
            {
                return View(invoice);
            }

            Dictionary<string, string> dic = _invoicesManager.AddInvoice(invoice);

            if (dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }

                return View(invoice);
            }
            if (dist.Trim().ToLower() == "pos")
            {
                return RedirectToAction("Index", "pos");
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: InvoiceController/Edit/5
        public IActionResult Edit(int id)
        {
            var DBInvoice = _invoicesManager.GetComplete(id);
            return View(DBInvoice);
        }


        // POST: InvoiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Invoice invoice)
        {
            Invoice DBInvoice = _unitOfWork.Invoices.FindById(id);
            try
            {
                if (DBInvoice == null)
                {
                    ModelState.AddModelError("", "الفاتورة غير موجودة.");
                    return View(invoice);
                }

                int checkResult = CheckInvoice(invoice);
                if (checkResult < 0)
                {
                    return View(DBInvoice);
                }

                Dictionary<string, string> dic = _invoicesManager.UpdateInvoice(invoice);

                if (dic.Count > 0)
                {
                    foreach (var item in dic)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }

                    return View(DBInvoice);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(DBInvoice);
            }
        }

        // GET: InvoiceController/Delete/5
        public IActionResult Delete(int id)
        {
            var invoice = _invoicesManager.GetComplete(id);
            return View(invoice);
        }

        // POST: InvoiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var DBInvoice = _unitOfWork.Invoices.FindById(id);
            try
            {
                if (DBInvoice == null)
                {
                    ModelState.AddModelError("", "الفاتورة غير موجودة.");
                    return View(DBInvoice);
                }
                _invoicesManager.DeleteInvoice(DBInvoice);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(DBInvoice);
            }
        }

        public int CheckInvoice(Invoice invoice)
        {
            if (invoice == null || invoice.Items == null || !invoice.Items.Any())
            {
                ModelState.AddModelError("", "لا يوجد أصناف.");
                return -1;
            }

            ModelState.Clear();

            var nameIdentifierClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
                ModelState.AddModelError("", "لم يتم العثور على معرف المستخدم. يرجى تسجيل الدخول مرة أخرى.");
            else
            {
                invoice.User = _unitOfWork.Users.Get(u => u.Id == int.Parse(nameIdentifierClaim.Value));
                if (false && invoice.User.Role.ToLower().Trim() == "seller" && !( invoice.Type == InvoiceType.Cash || invoice.Type == InvoiceType.Credit ))
                {
                    ModelState.AddModelError("", "ليس لديك صلاحية.");
                    return -1;
                }
            }
            return 0;
        }
    }
}
