using FluentValidation;
using GlobalBlue.CustomerManager.Application.ValueObjects;

namespace GlobalBlue.CustomerManager.Application.Update
{
    public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(command => command.NewFirstName).NotNull().NotEmpty();
            RuleFor(command => command.NewSurname).NotNull().NotEmpty();
            RuleFor(command => command.NewEmailAddress).NotNull().NotEmpty().Must(emailAddress => EmailAddress.IsValid(emailAddress));
            RuleFor(command => command.NewPassword).NotNull().NotEmpty();
        }
    }
}
