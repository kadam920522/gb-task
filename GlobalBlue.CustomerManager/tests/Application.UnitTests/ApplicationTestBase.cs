using GlobalBlue.CustomerManager.Application.Common.Abstract;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace GlobalBlue.CustomerManager.Application.UnitTests
{
    public abstract class ApplicationTestBase : IDisposable
    {
        protected readonly ISender _sender;

        protected readonly Mock<ICustomerStorage> _customerStorageMock;
        protected readonly Mock<IPasswordHasher> _passwordHasherMock;

        private readonly ServiceProvider _serviceProvider;

        public ApplicationTestBase()
        {
            _customerStorageMock = new Mock<ICustomerStorage>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            var services = new ServiceCollection();
            services.AddApplication();
            services.AddTransient(_ => _customerStorageMock.Object);
            services.AddTransient(_ => _passwordHasherMock.Object);

            _serviceProvider = services.BuildServiceProvider();
            _sender = _serviceProvider.GetService<ISender>();
        }

        public void Dispose() => _serviceProvider.Dispose();
    }
}
