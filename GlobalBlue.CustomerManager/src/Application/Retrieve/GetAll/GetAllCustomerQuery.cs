using GlobalBlue.CustomerManager.Application.Entities;
using MediatR;
using System.Collections.Generic;

namespace GlobalBlue.CustomerManager.Application.Retrieve.GetAll
{
    public sealed class GetAllCustomerQuery : IRequest<IEnumerable<Customer>>
    {
    }
}
