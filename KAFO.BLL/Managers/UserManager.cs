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
            return UOW.Users.Get(u =>
            ( u.Email == userName || u.PhoneNumber == userName ) && u.Password == password);
        }
        public List<User> GetAll()
        {
            return UOW.Users.GetAll().ToList();
        }
        public void Add(User user)
        {

            UOW.Users.Add(user);
            UOW.Save();
        }
        public User Get(int id)
        {
            return UOW.Users.FindById(id);

        }
        public bool CanUpdateByEmailOrPhone(string userInput, int userId)
        {
            var res = UOW.Users.Get(u => ( u.Email == userInput || u.PhoneNumber == userInput ) && u.Id != userId);
            return res == null ? true : false;
        }
        public User FindByEmailOrPhone(string userInput)
        {
            return UOW.Users.Get(u => u.Email == userInput || u.PhoneNumber == userInput);
        }
        public void Update(User user)
        {
            //all product data from repository
            UOW.Users.Update(user);
            UOW.Save();
        }
        public string Delete(int userID)
        {
            User user = Get(userID);
            if (user != null)
            {
                UOW.Users.Remove(user);
                UOW.Save();
            }
            else
            {
                return "المستخدم غير موجود";
            }
            return null;
        }

    }
}
