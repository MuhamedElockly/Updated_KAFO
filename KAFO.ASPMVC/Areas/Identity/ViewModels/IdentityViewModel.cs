using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Identity.ViewModels
{
    public class IdentityViewModel
    {
        public int Id { get; set; }
        [Display(Name = " ايميل المستخدم او رقم الموبايل")]
        public string userName { get; set; } = null!;

        [Display(Name = "كلمة المرور")]
        [DataType(DataType.Password)]
        public string password { get; set; } = null!;

        //[Display(Name = "تأكيد كلمة المرور")]
        //[DataType(DataType.Password)]
        //[Compare("password", ErrorMessage = "Passwords do not match")]
        //public string? confirmPassword { get; set; }

    }
}
