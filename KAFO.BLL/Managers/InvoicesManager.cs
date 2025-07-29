using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.Domain.Invoices;
using KAFO.Domain.Products;
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

        #region Complete Invoices
        public Dictionary<string, string> AddInvoice(Invoice invoice)
        {
            var Products = _unitOfWork.Products.GetAll(filter: p => p.IsActive);
            Dictionary<string, string> errorDic;
            errorDic = PrepareInvoice(invoice, Products);
            if (errorDic.Count > 0)
                return errorDic; // Return errors if any

            // custom logic or vlaidation
            switch (invoice.Type)
            {
                case InvoiceType.Cash:
                    errorDic = AddCashInvoice(invoice);
                    break;
                case InvoiceType.Credit:
                    errorDic = AddCreditInvoice(invoice);
                    break;
                case InvoiceType.Purchasing:
                    errorDic = AddPurchaseInvoice(invoice);
                    break;
                case InvoiceType.CashReturn:
                    errorDic = AddCashReturnInvoice(invoice);
                    break;
                case InvoiceType.CreditReturn:
                    errorDic = AddCreditReturnInvoice(invoice);
                    break;
                case InvoiceType.PurchasingReturn:
                    errorDic = AddPurchasingReturnInvoice(invoice);
                    break;
                default:
                    throw new NotImplementedException("Invoice type not implemented: " + invoice.Type);
            }

            if (errorDic.Count > 0)
                return errorDic; // Return errors if any

            invoice.CompleteInvoice();
            _unitOfWork.Invoices.Add(invoice);
            _unitOfWork.Save();

            return errorDic;
        }

        private Dictionary<string, string> PrepareInvoice(Invoice invoice, IEnumerable<Product> Products)
        {
            invoice.CreatedAt = DateTime.Now;
            var dic = new Dictionary<string, string>();

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
                if (item.UnitPurchasingPrice == 0)
                    item.UnitPurchasingPrice = item.Product.AveragePurchasePrice;
                if (item.UnitSellingPrice == 0)
                    item.UnitSellingPrice = item.Product.SellingPrice;
                validItems.Add(item);
                invoice.TotalInvoice += item.UnitSellingPrice * item.Quantity;
            }
            invoice.Items = validItems;

            if (invoice.Items == null || invoice.Items.Count == 0)
            {
                dic.Add("", "يجب إضافة صنف واحد على الأقل للفاتورة.");
                return dic;
            }
            return dic;
        }

        public Dictionary<string, string> AddCashInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            // No modification to item.UnitSellingPrice; return invoice as is
            return dic;
        }

        private Dictionary<string, string> AddPurchasingReturnInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            foreach (var item in invoice.Items)
            {
                if (item.Product.StockQuantity < item.Quantity)
                    dic.Add("", $"المنتج {item.Product.Name} رصيده اقل من المراد ارجاعه");
            }
            return dic;
        }

        public Dictionary<string, string> AddPurchaseInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            foreach (var item in invoice.Items)
            {
                // Update BoxPurchasePrice with the latest value from the invoice item - update the avg price in complete invoice
                if (item.Product.BoxPurchasePrice > 0)
                {
                    item.Product.BoxPurchasePrice = item.UnitPurchasingPrice * item.Product.BoxQuantity;
                    _unitOfWork.Products.Update(item.Product);
                }
            }
            return dic;
        }

        private Dictionary<string, string> AddCreditReturnInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            invoice.CustomerAccount = _unitOfWork.CustomerAccounts.Get(c => c.Id == invoice.CustomerAccountId);
            if (invoice.CustomerAccount!.TotalOwed < invoice.TotalInvoice) // && invoice.CustomerAccount.TotalPaid < invoice.TotalInvoice
                dic.Add("", "قيمة مديونية العميل اقل من قيمة الفاتورة");
            return dic;
        }

        private Dictionary<string, string> AddCashReturnInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            //foreach (var item in invoice.Items)
            //{
            //    if (item.UnitSellingPrice != item.Product.SellingPrice || item.UnitPurchasingPrice != item.Product.SellingPrice)
            //        dic.Add("", "لا يمكن تعديل السعر في فاتورة المرتجع");
            //}
            return dic;
        }

        private Dictionary<string, string> AddCreditInvoice(Invoice invoice)
        {
            var dic = new Dictionary<string, string>();
            // No modification to item.UnitSellingPrice; return invoice as is
            return dic;
        }
        #endregion

        #region Others
        //public List<Invoice> GetCompleteAll()
        //{
        //    return _appDBContext.Invoices
        //        .Include(i => i.User)
        //        .Include(i => i.CustomerAccount)
        //        .Include(i => i.Items)
        //        .ThenInclude(ii => ii.Product)
        //        .ToList();
        //}
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
        #endregion
    }
}