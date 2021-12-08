using GlobalBlue.CustomerManager.Application.Entites;
using MediatR;
using System.Collections.Generic;

namespace GlobalBlue.CustomerManager.Application.Retrieve.GetAll
{
    public sealed class GetAllCustomerQuery : IRequest<IEnumerable<Customer>>
    {
    }
}
