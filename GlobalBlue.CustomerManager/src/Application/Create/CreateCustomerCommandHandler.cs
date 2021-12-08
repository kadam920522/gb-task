using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entites;
using GlobalBlue.CustomerManager.Application.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Create
{
    internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerStorage _customerStorage;
        private readonly IPasswordHasher _passwordHasher;

        public CreateCustomerCommandHandler(ICustomerStorage customerStorage, IPasswordHasher passwordHasher)
        {
            _customerStorage = customerStorage;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerStorage.GetByEmailAddressAsync(request.EmailAddress);
            if (customer != null) throw new CustomerConflictException($"Customer already exists with the following e-mail address: {request.EmailAddress}");

            customer = Map(request);
            var newCustomerId = await _customerStorage.AddAsync(customer);
            return newCustomerId;
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
