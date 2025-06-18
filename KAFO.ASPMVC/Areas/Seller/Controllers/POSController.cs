using Kafo.DAL.Repository;
using KAFO.ASPMVC.Models;
using KAFO.Domain.Invoices;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Controllers
{
    [Area("Seller")]
    public class POSController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public POSController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.Products = _unitOfWork.Product.GetAll("Category", p => p.IsActive);
            ViewBag.Cats = _unitOfWork.Category.GetAll();
            ViewBag.Customers = _unitOfWork.CustomerAccount.GetAll();

            return View();
        }
        [ValidateAntiForgeryToken]
        public IActionResult Create(InvoiceViewModel invoice)
        {
            //move that to BLL
            var Products = _unitOfWork.Product.GetAll("Category", p => p.IsActive);
            var Cats = _unitOfWork.Category.GetAll();
            var Customers = _unitOfWork.CustomerAccount.GetAll();

            invoice.CreatedAt = DateTime.Now;
            invoice.CustomerAccount = Customers.FirstOrDefault(c => c.Id == invoice.CustomerAccount?.Id);
            //invoice.User = _unitOfWork.User.FindById(User.Identity.Name!);
            invoice.User = new Domain.Users.User("s", "s", "s", "s");
            ModelState.Clear();
            foreach (var item in invoice.Items)
            {
                item.Invoice = invoice;
                item.Product = Products.FirstOrDefault(p => p.Id == item.ProductId);
                item.UnitSellingPrice = item.Product.SellingPrice;
                item.UnitPurchasingPrice = item.Product.AveragePurchasePrice;
                if (item.Quantity > item.Product.StockQuantity)
                {
                    ModelState.AddModelError("", $"لا يمكن شراء عدد {item.Quantity} قطع من المنتج {item.Product.Name} لان اجمالي ما يزجد في المتجر هو {item.Product.StockQuantity}");
                }
            }
            if (ModelState.IsValid)
            {
                if (invoice.Type == InvoiceType.Cash)
                {
                    CashInvoice inv = invoice.ToCashInvoice();
                    inv.CompleteInvoice();
                    _unitOfWork.CashInvoice.Add(inv);
                    _unitOfWork.Save();
                }
                else if (invoice.Type == InvoiceType.Credit)
                {
                    CreditInvoice inv = invoice.ToCreditInvoice();
                    inv.CompleteInvoice();
                    _unitOfWork.CreditInvoice.Add(inv);
                    _unitOfWork.Save();
                }
                else if (invoice.Type == InvoiceType.Purchasing)
                {
                    PurchasingInvoice inv = invoice.ToPurchasingInvoice();
                    inv.CompleteInvoice();
                    _unitOfWork.PurchasingInvoice.Add(inv);
                    _unitOfWork.Save();
                }
            }
            //else if (invoice.Type == InvoiceType.CashReturn)
            //{
            //    CashReturnInvoice inv = invoice.ToCashReturnInv();
            //    _unitOfWork.CashReturnInvoice.Add(inv);
            //    _unitOfWork.Save();
            //}
            //else if (invoice.Type == InvoiceType.CreditReturn)
            //{
            //    CreditReturnInvoice inv = invoice.ToCreditReturnInv();
            //    _unitOfWork.CreditReturnInvoice.Add(inv);
            //    _unitOfWork.Save();
            //}
            //else if (invoice.Type == InvoiceType.PurchasingReturn)
            //{
            //    PurchasingReturnInvoice inv = invoice.ToPurchasingReturnInv();
            //    _unitOfWork.PurchasingReturnInvoice.Add(inv);
            //    _unitOfWork.Save();
            //}

            return RedirectToAction(nameof(Index));
        }
    }
}
