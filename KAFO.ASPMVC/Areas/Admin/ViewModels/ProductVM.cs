using KAFO.Domain.Products;

namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class ProductVM
	{
		public int Id { set; get; }
		public string Name { set; get; }
		public string? ImageUrl { set; get; }
		public decimal SellingPrice { set; get; }
		public decimal StockQuantity { get; set; }
		public bool IsActive { get; set; }
		public virtual Category? Category { set; get; }
	}
}
