namespace TaskManager.Domain.UserAggregate
{
    public class User : Entity
    {
        public User(
            string name, 
            string email, 
            string password,
            DateTime createdDate)
        {
            Name = name;
            Email = email;
            Password = password;
            CreatedDate = createdDate;
        }

        public string Name{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
