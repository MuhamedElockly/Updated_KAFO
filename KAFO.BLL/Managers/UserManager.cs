using Kafo.DAL.Repository;
using KAFO.Domain.Products;

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
		public List<User> GetAll()
		{
			return UOW.User.GetAll().ToList();
		}
		public void Add(User user)
		{

			UOW.User.Add(user);
			UOW.Save();
		}
		public User FindByEmailOrPhone(string userInput)
		{
			return UOW.User.Get(u => u.Email == userInput || u.PhoneNumber == userInput);
		}

	}
}
