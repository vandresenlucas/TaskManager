using TaskManager.CrossCutting.Extensions;
using TaskManager.Domain.UserAggregate;

namespace TasktManager.Application.Tests.Models
{
    public static class UserModelTest
    {
        public static User UserDefault()
        {
            var user = new User(
                "Test User",
                "test@example.com",
                "P@ssword123".CalculateSHA256Hash(),
                DateTime.Now);

            user.Id = Guid.NewGuid();

            return user;
        }
    }
}
