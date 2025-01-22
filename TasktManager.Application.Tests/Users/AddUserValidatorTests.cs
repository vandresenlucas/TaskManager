using FluentValidation.TestHelper;
using TaskManager.Application.Users;
using TasktManager.Application.Tests.Models;

namespace TaskManager.Tests.Application.Users
{
    public class AddUserValidatorTests
    {
        private readonly AddUserValidator _validator;

        public AddUserValidatorTests()
        {
            _validator = new AddUserValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenFullNameIsEmpty()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithoutName();

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("O campo 'FullName' deve ser preenchido!!");
        }

        [Fact]
        public void ShouldHaveErrorWhenFullNameExceedsMaxLength()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithFullName(new string('A', 101));

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("O campo 'FullName' deve ter no máximo 100 caracteres!!");
        }

        [Fact]
        public void ShouldHaveErrorWhenFullNameIsTooShort()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithFullName("asd");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorMessage("O campo 'FullName' deve ter no mínimo 5 caracteres!!");
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsEmpty()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithoutEmail();

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("O campo 'Email' deve ser preenchido!!");
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsEmpty()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithoutPassword();

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("O campo 'Password' deve ser preenchido!!");
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsInvalid()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandWithPassword("teste123");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("O campo 'Password' deve ter no mínimo 8 caractere, letras maiúsculas, minúsculas, números e caracteres especiais!!");
        }

        [Fact]
        public void Should_NotHaveError_When_AllFieldsAreValid()
        {
            // Arrange
            var command = UserCommandModelTest.UserCommandDefault();

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
