using System.ComponentModel.DataAnnotations;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        [Required(ErrorMessage = "اسم الفئة مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون اسم الفئة بين 3 و100 حرف")]
        [Display(Name = "اسم الفئة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وصف الفئة مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز الوصف 500 حرف")]
        [Display(Name = "الوصف")]
        public string Description { get; set; }
        public int Id { get; set; }
    }
}
