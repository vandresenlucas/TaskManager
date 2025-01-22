using TaskManager.Application.Users.CommandHandlers;
using TaskManager.Domain.UserAggregate;

namespace TasktManager.Application.Tests.Models
{
    public static class UserCommandModelTest
    {
        public static AddUserCommand UserCommandDefault()
            => new AddUserCommand
            {
                FullName = "Valid Name",
                Email = "email@example.com",
                Password = "Valid@123"
            };

        public static AddUserCommand UserCommandWithoutName()
            => new AddUserCommand
            {
                Email = "email@example.com",
                Password = "Valid@123"
            };

        public static AddUserCommand UserCommandWithFullName(string name)
            => new AddUserCommand
            {
                FullName = name,
                Email = "email@example.com",
                Password = "Valid@123"
            };

        public static AddUserCommand UserCommandWithoutEmail()
            => new AddUserCommand
            {
                FullName = "Valid Name",
                Password = "Valid@123"
            };

        public static AddUserCommand UserCommandWithoutPassword()
            => new AddUserCommand
            {
                FullName = "Valid Name",
                Password = "",
                Email = "email@example.com"
            };

        public static AddUserCommand UserCommandWithPassword(string password)
            => new AddUserCommand
            {
                FullName = "Valid Name",
                Email = "email@example.com",
                Password = password
            };

    }
}
