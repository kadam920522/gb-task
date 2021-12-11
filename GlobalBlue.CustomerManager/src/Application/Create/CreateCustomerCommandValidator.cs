using FluentValidation;
using GlobalBlue.CustomerManager.Application.ValueObjects;

namespace GlobalBlue.CustomerManager.Application.Create
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(command => command.FirstName).NotNull().NotEmpty();
            RuleFor(command => command.Surname).NotNull().NotEmpty();
            RuleFor(command => command.EmailAddress).NotNull().NotEmpty().Must(emailAddress => EmailAddress.IsValid(emailAddress));
            RuleFor(command => command.Password).NotNull().NotEmpty();
        }
    }
}
