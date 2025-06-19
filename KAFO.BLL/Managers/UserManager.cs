using Kafo.DAL.Repository;
using KAFO.Domain.Users;

namespace KAFO.BLL.Managers
{
	public class UserManager : Manager<User>
	{
		IUnitOfWork UOW = null;
		public UserManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
		public User? Get(string userName, string password)
		{
			return UOW.User.Get(u =>
			(u.Email == userName || u.PhoneNumber == userName) && u.Password == password);
		}
		
	}
}
