using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Update
{
    internal sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        private readonly ICustomerStorage _customerStorage;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateCustomerCommandHandler(ICustomerStorage customerStorage, IPasswordHasher passwordHasher)
        {
            _customerStorage = customerStorage;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerStorage.GetById(request.CustomerId);

            if (customer is null) throw new CustomerNotFoundException(request.CustomerId);

            if(customer.EmailAddress != request.NewEmailAddress)
            {
                var existingCustomer = await _customerStorage.GetByEmailAddressAsync(request.NewEmailAddress);
                if (existingCustomer != null) throw new CustomerEmailAddressConflictException(request.NewEmailAddress);
            }

            customer.EmailAddress = request.NewEmailAddress;
            customer.FirstName = request.NewFirstName;
            customer.Surname = request.NewSurname;
            customer.Password = _passwordHasher.Hash(request.NewPassword);

            await _customerStorage.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
