//using KAFO.Domain.Users;

//namespace KAFO.Domain.Invoices.ReturnInvoices
//{
//    public class CashReturnInvoice(DateTime createdAt, User user) : Invoice(createdAt, user)
//    {
//        public override void CompleteInvoice()
//        {
//            base.CompleteInvoice();
//            foreach (var item in this.Items)
//            {
//                // Increase The StockQuantity 
//                item.Product.IncreaseStockQuantity(item.Quantity);
//            }
//        }
//    }
//}
