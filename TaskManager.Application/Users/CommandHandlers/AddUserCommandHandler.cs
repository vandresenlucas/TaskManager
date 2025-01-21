using MediatR;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Tasks.CommandHandlers.AddTask;
using TaskManager.CrossCutting.Contracts;
using TaskManager.CrossCutting.Extensions;
using TaskManager.Domain.UserAggregate;

namespace TaskManager.Application.Users.CommandHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddTaskCommandHandler> _logger;

        public AddUserCommandHandler(IUserRepository userRepository, ILogger<AddTaskCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            User user = request;

            if (await _userRepository.VerifyUserExists(request.Email))
                return new Result(false, $"Email {request.Email} já está registrado no sistema!!");

            if (!request.Password.ValidatePassword())
                return new Result(false, $"A senha deve ter no mínimo 8 caractere, letras maiúsculas, minúsculas, números e caracteres especiais!!");

            var newUser = await _userRepository.AddAsync(user);

            return new Result(response: newUser);
        }
    }
}
