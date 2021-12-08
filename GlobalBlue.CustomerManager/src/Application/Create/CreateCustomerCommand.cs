using GlobalBlue.CustomerManager.Application.Entities;
using MediatR;

namespace GlobalBlue.CustomerManager.Application.Create
{
    public sealed class CreateCustomerCommand : IRequest<Customer>
    {
        public CreateCustomerCommand(string firstName, string surname, string emailAddress, string password)
        {
            FirstName = firstName;
            Surname = surname;
            EmailAddress = emailAddress;
            Password = password;
        }

        public string FirstName { get; }

        public string Surname { get; }

        public string EmailAddress { get; }

        public string Password { get; }
    }
}
