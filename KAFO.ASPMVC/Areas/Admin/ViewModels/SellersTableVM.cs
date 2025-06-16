namespace KAFO.ASPMVC.Areas.Admin.ViewModels
{
	public class UsersTableVM
	{
		public List<UserVM> Users { get; set; }
		public int TotalUsersPages { get; set; }
		public int CurrentUserPage { get; set; }
	}

}



