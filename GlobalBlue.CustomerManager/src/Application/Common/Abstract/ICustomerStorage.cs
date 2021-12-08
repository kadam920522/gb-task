using GlobalBlue.CustomerManager.Application.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Common.Abstract
{
    public interface ICustomerStorage
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer> GetByEmailAddressAsync(string emailAddress);

        Task<Customer> GetById(int id);

        Task<int> AddAsync(Customer newCustomer);

        Task UpdateAsync(Customer updatedCustomer);

    }
}
