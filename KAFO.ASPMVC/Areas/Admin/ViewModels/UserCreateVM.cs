using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class UserCreateVM
    {
        public int Id { get; set; } = 0;

        [Display(Name = "اسم المستخدم")]
        public string Name { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "رقم الهاتف يجب أن يحتوي على أرقام فقط")]
        public string Phone { get; set; }
		[Display(Name = "النوع")]
		[Required(ErrorMessage = "النوع مطلوب")]
		public string Gender { get; set; }

		[Display(Name = "الدور")]
        [Required(ErrorMessage = "الدور مطلوب")]
        public string Role { get; set; }

        [Display(Name = "كلمة المرور")]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        public string Password { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }
    }
}