using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Application.Users.CommandHandlers
{
    [SwaggerSchema(Description = "Representa os dados para adicionar um novo usuário.")]
    public class AddUserCommand : IRequest<Result>
    {
        [SwaggerSchema("Nome completo do usuário")]
        public string FullName { get; set; }
        [SwaggerSchema("Endereço de e-mail do usuário")]
        public string Email { get; set; }
        [SwaggerSchema("Senha do usuário")]
        public string Password { get; set; }

        public static implicit operator User(AddUserCommand command)
        {
            if (command == null)
                return null;

            return new(
                command.FullName, 
                command.Email, 
                command.Password, 
                DateTime.Now);
        }
    }
}
