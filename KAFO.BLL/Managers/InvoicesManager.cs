using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace KAFO.BLL.Managers
{
    public class InvoicesManager : IManager<Invoice>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly AppDBContext _appDBContext;

        public InvoicesManager(IUnitOfWork unitOfWork, AppDBContext appDBContext)
        {
            _unitOfWork = unitOfWork;
            _appDBContext = appDBContext;
        }

        public Dictionary<string, string> AddInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();

            var Products = _unitOfWork.Products.GetAll(filter: p => p.IsActive);
            //var Cats = _unitOfWork.Categories.GetAll();
            var Customers = _unitOfWork.CustomerAccounts.GetAll();

            if (invoice.Type == InvoiceType.Credit && invoice.CustomerAccountId == null)
            {
                dic.Add("", "يجب اختيار حساب العميل");
                return dic;
            }
            if (invoice.Type == InvoiceType.Credit && invoice.CustomerAccountId != null)
            {
                invoice.CustomerAccount = Customers.FirstOrDefault(c => c.Id == invoice.CustomerAccountId);
                if (invoice.CustomerAccount == null)
                {
                    dic.Add("", "حساب العميل غير موجود");
                    return dic;
                }
            }

            // Filter out invalid items (no product selected or invalid product)
            var validItems = new List<InvoiceItem>();
            foreach (var item in invoice.Items)
            {
                if (item == null || item.ProductId == 0)
                {
                    continue; // skip invalid
                }
                var product = Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    continue; // skip invalid
                }
                item.Invoice = invoice;
                item.Product = product;
                item.UnitSellingPrice = product.SellingPrice;
                item.UnitPurchasingPrice = product.AveragePurchasePrice;
                validItems.Add(item);
            }
            invoice.Items = validItems;

            if (invoice.Items == null || invoice.Items.Count == 0)
            {
                dic.Add("", "يجب إضافة صنف واحد على الأقل للفاتورة.");
                return dic;
            }

            if (dic.Count == 0)
            {
                invoice.CompleteInvoice();
                _unitOfWork.Invoices.Add(invoice);
                _unitOfWork.Save();
            }
            return dic;
        }
        public List<Invoice> GetCompleteAll()
        {
            return _appDBContext.Invoices
                .Include(i => i.User)
                .Include(i => i.CustomerAccount)
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Product)
                .ToList();
        }
        public Invoice? GetComplete(int id)
        {
            return _appDBContext.Invoices
                .Where(i => i.Id == id)
                .Include(i => i.User)
                .Include(i => i.CustomerAccount)
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Product)
                .FirstOrDefault();
        }

        public Dictionary<string, string> UpdateInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public void DeleteInvoice(Invoice dBInvoice)
        {
            throw new NotImplementedException();
        }
    }
}