using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GlobalBlue.CustomerManager.Persistence
{
    public static class ApplyMigrationsExtension
    {
        public static void ApplyMigrations(this IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerManagerDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
