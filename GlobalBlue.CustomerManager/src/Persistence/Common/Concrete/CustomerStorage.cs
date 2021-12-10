using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Persistence.Common.Concrete
{
    internal sealed class CustomerStorage : ICustomerStorage
    {
        private readonly CustomerManagerDbContext _dbContext;

        public CustomerStorage(CustomerManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> AddAsync(Customer newCustomer)
        {
            await _dbContext.Customers.AddAsync(newCustomer);
            await _dbContext.SaveChangesAsync();

            return newCustomer;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.AsNoTracking().ToArrayAsync();
        }

        public async Task<Customer> GetByEmailAddressAsync(string emailAddress)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(customer => customer.EmailAddress == emailAddress);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(customer => customer.Id == id);
        }

        public async Task<Customer> UpdateAsync(Customer updatedCustomer)
        {
            _dbContext.Customers.Update(updatedCustomer);
            await _dbContext.SaveChangesAsync();

            return updatedCustomer;
        }
    }
}
