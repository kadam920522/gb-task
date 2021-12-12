using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.Persistence
{
    internal sealed class CustomerManagerDbContext : DbContext
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
            modelBuilder.Entity<Customer>().Property(c => c.Password).IsRequired();
            modelBuilder
                .Entity<Customer>()
                .Property(c => c.EmailAddress)
                .HasConversion(
                    convertToProviderExpression: value => value.ToString(),
                    convertFromProviderExpression: value => value)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>().HasIndex(c => c.EmailAddress).IsUnique();

            modelBuilder.Entity<Customer>().UseXminAsConcurrencyToken();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new CustomerChangedExcepion(ex);
            }
        }
    }
}
