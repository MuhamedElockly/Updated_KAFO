namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class AdminHomeVM
	{
		public double TotalProfitToday { get; set; }
		public double TotalSellsToday { get; set; }
		public int TotalProductSoldToday { get; set; }
		public string AdminName { get; set; }
		public ProductsTableVM Products { set; get; }
	}
}
