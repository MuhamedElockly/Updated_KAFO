namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class CategoriesTableVM
	{
		public List<CategoryVM> Categories { get; set; }
		public int TotalCategoriesPages { get; set; }
		public int CurrentCategoryPage { get; set; }
	}
}
