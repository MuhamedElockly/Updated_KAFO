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
        public User Get(int id)
        {
            return UOW.User.FindById(id);

        }
        public bool CanUpdateByEmailOrPhone(string userInput, int userId)
        {
            var res = UOW.User.Get(u => (u.Email == userInput || u.PhoneNumber == userInput) && u.Id != userId);
            return res == null ? true : false;
        }
        public User FindByEmailOrPhone(string userInput)
        {
            return UOW.User.Get(u => u.Email == userInput || u.PhoneNumber == userInput);
        }
        public void Update(User user)
        {
            //all product data from repository
            UOW.User.Update(user);
            UOW.Save();
        }
        public void Delete(User user)
        {

            UOW.User.Remove(user);
            UOW.Save();
        }

    }
}
