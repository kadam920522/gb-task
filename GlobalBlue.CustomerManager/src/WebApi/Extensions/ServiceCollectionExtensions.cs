using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.WebApi.Common.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalBlue.CustomerManager.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPasswordHasher(this IServiceCollection services) =>
            services.AddTransient<IPasswordHasher, PasswordHasher>();
    }
}
