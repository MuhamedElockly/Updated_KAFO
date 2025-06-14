namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class SellersTableVM
	{
		public List<SellerVM> Sellers { get; set; }
		public int TotalSellersPages { get; set; }
		public int CurrentSellerPage { get; set; }
	}

}



