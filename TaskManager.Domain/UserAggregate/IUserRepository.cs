namespace TaskManager.Domain.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> ValidateCredentials(UserCredentials credentials);
        Task<User> UpdateRefreshToken(User user);
    }
}
