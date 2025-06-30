using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class ProfileVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون الاسم بين 3 و100 حرف")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "رقم الهاتف يجب أن يحتوي على 11 رقم فقط")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; }

        [Display(Name = "النوع")]
        public string Gender { get; set; }

        [Display(Name = "الدور")]
        public string Role { get; set; }

        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        [Display(Name = "كلمة المرور الجديدة")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }
    }
} 