using KAFO.Domain.Statics;

namespace KAFO.Domain.Users
{
    public class User : Base, ISoftDelete
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Password { set; get; }
        public string Role { set; get; }
        public string Gender { set; get; }
        public string? Email { get; set; }
        public string? PhoneNumber { set; get; }
        public bool IsDeleted { set; get; }

        public User()
        {

        }

        public User(string name, string? email, string role, string gender, string? phoneNumber = null)
        {
            if (name == null || role == null || gender == null)
                throw new ArgumentNullException(Messages.ArgumentNullException);

            Name = name;
            Role = role;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
