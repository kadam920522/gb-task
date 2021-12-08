using GlobalBlue.CustomerManager.Application.Entities;
using MediatR;

namespace GlobalBlue.CustomerManager.Application.Retrieve.GetById
{
    public sealed class GetCustomerByIdQuery : IRequest<Customer>
    {
        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
