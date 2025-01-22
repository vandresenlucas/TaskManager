using FluentValidation;
using TaskManager.Application.Users.CommandHandlers;
using TaskManager.CrossCutting.Extensions;

namespace TaskManager.Application.Users
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserValidator()
        {
            RuleFor(command => command.FullName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("O campo 'FullName' deve ser preenchido!!")
                .NotEmpty()
                .WithMessage("O campo 'FullName' deve ser preenchido!!")
                .MaximumLength(100)
                .WithMessage("O campo 'FullName' deve ter no máximo 100 caracteres!!")
                .MinimumLength(5)
                .WithMessage("O campo 'FullName' deve ter no mínimo 5 caracteres!!");

            RuleFor(command => command.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo 'Email' deve ser preenchido!!");

            RuleFor(command => command.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo 'Password' deve ser preenchido!!")
                .Must(pass => pass.ValidatePassword())
                .WithMessage("O campo 'Password' deve ter no mínimo 8 caractere, letras maiúsculas, minúsculas, números e caracteres especiais!!");
        }
    }
}
