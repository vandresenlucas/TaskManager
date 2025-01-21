using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.CrossCutting.Contracts;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Application.Authentication.CommandHandlers
{
    [SwaggerSchema("Representa os dados necessários para autenticação de um usuário.")]
    public class AuthenticationCommand : IRequest<Result>
    {
        [SwaggerSchema(Description = "E-mail do usuário. Esse campo é obrigatório e único.", Nullable = false)]
        public string Email { get; set; }
        [SwaggerSchema(Description = "Senha do usuário. Esse campo é obrigatório.", Nullable = false)]
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
