using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class ProfileVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "النوع مطلوب")]
        [Display(Name = "النوع")]
        public string Gender { get; set; }

        [Display(Name = "الدور")]
        public string? Role { get; set; }

        [Display(Name = "كلمة المرور الجديدة")]
        public string? NewPassword { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        public string? ConfirmPassword { get; set; }
    }
} 