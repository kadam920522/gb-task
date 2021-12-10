using GlobalBlue.CustomerManager.Application.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Common.Abstract
{
    public interface ICustomerStorage
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer> GetByEmailAddressAsync(string emailAddress);

        Task<Customer> GetByIdAsync(int id);

        Task<Customer> AddAsync(Customer newCustomer);

        Task<Customer> UpdateAsync(Customer updatedCustomer);

    }
}
