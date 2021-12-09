using FluentValidation;
using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Common.Behaviors;
using GlobalBlue.CustomerManager.Application.Common.Concrete;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GlobalBlue.CustomerManager.Application.UnitTests")]

namespace GlobalBlue.CustomerManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient<ICustomerStorage, InMemoryCustomerStorage>();

            return services;
        }
    }
}
