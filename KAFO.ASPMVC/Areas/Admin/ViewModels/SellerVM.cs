namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class SellerVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Role { get; set; } = "Seller";
	}

}



