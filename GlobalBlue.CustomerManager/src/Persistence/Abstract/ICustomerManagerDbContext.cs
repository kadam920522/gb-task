using GlobalBlue.CustomerManager.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Persistence.Abstract
{
    internal interface ICustomerManagerDbContext
    {
        DbSet<Customer> Customers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
