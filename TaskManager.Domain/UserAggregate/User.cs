namespace TaskManager.Domain.UserAggregate
{
    public class User : Entity
    {
        public User(
            string fullName, 
            string email, 
            string password,
            DateTime createdDate)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            CreatedDate = createdDate;
        }

        public string FullName{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
    }
}
