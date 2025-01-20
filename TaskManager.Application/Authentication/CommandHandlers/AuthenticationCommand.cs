using MediatR;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Application.Authentication.CommandHandlers
{
    public class AuthenticationCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public static implicit operator UserCredentials(AuthenticationCommand command)
        {
            if (command == null)
            {
                return null;
            }

            return new()
            {
                Email = command.Email,
                Password = command.Password
            };
        }
    }
}
