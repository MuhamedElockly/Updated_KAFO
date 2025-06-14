//using KAFO.Domain.Users;

//namespace KAFO.Domain.Invoices.ReturnInvoices
//{
//    public class CreditReturnInvoice(DateTime createdAt, User user, CustomerAccount customerAccount) : CreditInvoice(createdAt, user, customerAccount)
//    {
//        public override void CompleteInvoice()
//        {
//            CalculateTotalInvoice();
//            foreach (var item in Items)
//            {
//                // Increase The StockQuantity 
//                item.Product.IncreaseStockQuantity(item.Quantity);
//            }
//            CustomerAccount.TotalOwed -= TotalInvoice;
//        }
//    }
//}
