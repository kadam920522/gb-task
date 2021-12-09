﻿using GlobalBlue.CustomerManager.Persistence.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalBlue.CustomerManager.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerManagerDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));
            services.AddScoped<ICustomerManagerDbContext>(provider => provider.GetService<CustomerManagerDbContext>());

            return services;
        }
    }
}
