namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomerDisplayVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Gender { get; set; }
        public decimal Balance { get; set; }
        public decimal Credit { get; set; }
    }
} 