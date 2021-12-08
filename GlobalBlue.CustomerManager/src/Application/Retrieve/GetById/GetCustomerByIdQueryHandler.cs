using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Retrieve.GetById
{
    internal sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerStorage _customerStorage;

        public GetCustomerByIdQueryHandler(ICustomerStorage customerStorage)
        {
            _customerStorage = customerStorage;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerStorage.GetById(request.Id);
            if (customer is null) throw new CustomerNotFoundException(request.Id);

            return customer;
        }
    }
}
