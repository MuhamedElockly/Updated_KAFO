//using KAFO.Domain.Products;
//using KAFO.Domain.Users;

//namespace KAFO.Domain.Invoices.ReturnInvoices
//{
//    public class PurchasingReturnInvoice : Invoice
//    {
//        public PurchasingInvoice ReferenceInvoice { get; set; }
//        public PurchasingReturnInvoice(DateTime createdAt, User user, PurchasingInvoice referenceInvoice) : base(createdAt, user)
//        {
//            ReferenceInvoice = referenceInvoice;
//        }

//        public override void CompleteInvoice()
//        {
//            //base.CompleteInvoice();
//            //foreach (var item in Items)
//            //{
//            //updateProductPrice(item.Product, item.Quantity);
//            //// update products Quantity
//            //item.Product.DecreaseStockQuantity(item.Quantity);

//            //}
//        }

//        private void updateProductPrice(Product returnProduct, int returnQuantity, int invoiceNumber = 0)
//        {
//            //get all past Invoices with this product

//            //return the product and update Price and see if there more quantities to return 
//            //if the returnQuantity > 0 make requrtion with the privious invoice 
//        }
//    }
//}
