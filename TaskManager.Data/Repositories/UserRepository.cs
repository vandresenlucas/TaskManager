using Microsoft.EntityFrameworkCore;
using TaskManager.CrossCutting.Extensions;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TaskManagerContext context) : base(context) { }

        public async Task<User> UpdateRefreshToken(User user)
            => await UpdateAsync(user);

        public async Task<User> ValidateCredentials(UserCredentials credentials)
        {
            var password = credentials.Password.CalculateSHA256Hash();

            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == credentials.Email && u.Password.CalculateSHA256Hash() == password);
        }

        public async Task<bool> VerifyUserExists(string email)
            => await _context.Set<User>().AnyAsync(u => u.Email == email);
    }
}
