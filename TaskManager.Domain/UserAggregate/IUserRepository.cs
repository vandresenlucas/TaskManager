namespace TaskManager.Domain.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> ValidateCredentials(UserCredentials credentials);
        Task<User> UpdateRefreshToken(User user);
        Task<bool> VerifyUserExists(string email);
    }
}
