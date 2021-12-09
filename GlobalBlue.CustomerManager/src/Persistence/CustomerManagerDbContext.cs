using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Persistence.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GlobalBlue.CustomerManager.Persistence
{
    internal sealed class CustomerManagerDbContext : DbContext, ICustomerManagerDbContext
    {
        public CustomerManagerDbContext(DbContextOptions<CustomerManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Surname).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.EmailAddress).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Password).IsRequired();
        }
    }
}
