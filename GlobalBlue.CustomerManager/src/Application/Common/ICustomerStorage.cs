using Application.Entites;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface ICustomerStorage
    {
        Task<int> AddAsync(Customer newCustomer);

        Task<Customer> GetByEmailAddressAsync(string emailAddress);
    }
}
