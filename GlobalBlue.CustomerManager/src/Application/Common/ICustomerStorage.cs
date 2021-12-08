using GlobalBlue.CustomerManager.Application.Entites;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Application.Common
{
    public interface ICustomerStorage
    {
        Task<int> AddAsync(Customer newCustomer);

        Task<Customer> GetByEmailAddressAsync(string emailAddress);

        Task<Customer> GetById(int id);

        Task UpdateAsync(Customer updatedCustomer);
    }
}
