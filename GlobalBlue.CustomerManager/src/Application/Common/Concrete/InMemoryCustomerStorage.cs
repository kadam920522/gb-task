using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Common.Concrete
{
    internal sealed class InMemoryCustomerStorage : ICustomerStorage
    {
        private static readonly ICollection<Customer> _customers = new HashSet<Customer>();

        public Task<Customer> AddAsync(Customer newCustomer)
        {
            var newId = _customers.Any() ? _customers.Select(customer => customer.Id).Max() + 1 : 1;
            newCustomer.Id = newId;
            _customers.Add(newCustomer);

            return Task.FromResult(newCustomer);
        }

        public Task<IEnumerable<Customer>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Customer>>(_customers.ToArray());

        public Task<Customer> GetByEmailAddressAsync(string emailAddress) =>
            Task.FromResult(_customers.FirstOrDefault(customer => customer.EmailAddress == emailAddress));

        public Task<Customer> GetByIdAsync(int id) =>
            Task.FromResult(_customers.FirstOrDefault(customer => customer.Id == id));

        public Task<Customer> UpdateAsync(Customer updatedCustomer)
        {
            var customer = _customers.FirstOrDefault(customer => customer.Id == updatedCustomer.Id);
            if(customer != null)
            {
                customer.FirstName = updatedCustomer.FirstName;
                customer.Surname = updatedCustomer.Surname;
                customer.EmailAddress = updatedCustomer.EmailAddress;
                customer.Password = updatedCustomer.Password;
            }

            return Task.FromResult(customer);
        }
    }
}
