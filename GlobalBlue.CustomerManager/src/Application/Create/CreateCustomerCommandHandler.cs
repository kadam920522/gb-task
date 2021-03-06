using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Create
{
    internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerStorage _customerStorage;
        private readonly IPasswordHasher _passwordHasher;

        public CreateCustomerCommandHandler(ICustomerStorage customerStorage, IPasswordHasher passwordHasher)
        {
            _customerStorage = customerStorage;
            _passwordHasher = passwordHasher;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerStorage.GetByEmailAddressAsync(request.EmailAddress);
            if (customer != null) throw new CustomerEmailAddressConflictException(request.EmailAddress);

            customer = Map(request);
            customer = await _customerStorage.AddAsync(customer);
            return customer;
        }

        private Customer Map(CreateCustomerCommand command) => new Customer
        {
            EmailAddress = command.EmailAddress,
            FirstName = command.FirstName,
            Surname = command.Surname,
            Password = _passwordHasher.Hash(command.Password)
        };
    }
}
