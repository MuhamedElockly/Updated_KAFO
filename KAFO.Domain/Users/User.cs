using KAFO.Domain.Statics;

namespace KAFO.Domain.Users
{
    public class User : Base, ISoftDelete
    {
        public int Id { private set; get; }
        public string Name { set; get; }
        public string Password { set; get; }
        public string Role { private set; get; }
        public string Gender { set; get; }
        public string Email { get; set; }
        public string? PhoneNumber { set; get; }
        public bool IsDeleted { set; get; }

        protected User()
        {

        }

        public User(string name, string email, string role, string gender, string? phoneNumber = null)
        {
            if (name == null || role == null || email == null || gender == null)
                throw new ArgumentNullException(Messages.ArgumentNullException);

            Name = name;
            Role = role;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
