

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class ProductsTableVM
	{
		public List<ProductVM> Products { get; set; }
		public int TotalProductPages { get; set; }
		public int CurrentProductPage { get; set; }
	}
}
