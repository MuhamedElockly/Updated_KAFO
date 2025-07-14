namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; } // Not required
        public string Gender { get; set; }
    }
} 