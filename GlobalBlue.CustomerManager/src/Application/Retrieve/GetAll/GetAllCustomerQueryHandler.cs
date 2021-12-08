using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entites;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Retrieve.GetAll
{
    internal sealed class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, IEnumerable<Customer>>
    {
        private readonly ICustomerStorage _customerStorage;

        public GetAllCustomerQueryHandler(ICustomerStorage customerStorage)
        {
            _customerStorage = customerStorage;
        }

        public Task<IEnumerable<Customer>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken) =>
            _customerStorage.GetAllAsync();
    }
}
