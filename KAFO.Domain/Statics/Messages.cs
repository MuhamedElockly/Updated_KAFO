namespace KAFO.Domain.Statics
{
    public static class Messages
    {
        public static readonly string NegativeQuantityException = "Quantity is less than Zero or bigger than the stock quantity";
        public static readonly string ArgumentNullException = "Argument can not be Null";
        public static readonly string EmptyInvoice = "You can not close invoice with no item";
        public const string RequiredMessage = "هذا الحقل مطلوب";
    }
}
