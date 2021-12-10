using FluentAssertions;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Retrieve.GetAll;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.Application.UnitTests.Retrieve.GetAll
{
    public class GetAllCustomerQueryTest : ApplicationTestBase
    {
        [Fact]
        public async Task ShouldReturnAllTheCustomers()
        {
            // Arrange
            var expectedCustomers = new[]
            {
                new Customer(), new Customer(), new Customer()
            };

            _customerStorageMock.Setup(storage => storage.GetAllAsync()).ReturnsAsync(expectedCustomers);

            // Act
            var customers = await _sender.Send(new GetAllCustomerQuery());

            // Assert
            customers.Should().BeEquivalentTo(expectedCustomers);
        }
    }
}
