using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CreditCustomerAddVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")] 
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")] 
        public string Phone { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "النوع مطلوب")] 
        public string Gender { get; set; }
    }
} 