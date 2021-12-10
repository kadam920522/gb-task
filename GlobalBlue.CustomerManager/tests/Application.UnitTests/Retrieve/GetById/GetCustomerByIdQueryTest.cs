using FluentAssertions;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using GlobalBlue.CustomerManager.Application.Retrieve.GetById;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.Application.UnitTests.Retrieve.GetById
{
    public class GetCustomerByIdQueryTest : ApplicationTestBase
    {
        [Fact]
        public void ShouldThrowCustomerNotFoundException_WhenNoSuchCustomerWithTheSpecifiedId()
        {
            // Arrange
            const int NOT_EXISTING_CUSTOMER_ID = 5;
            var query = new GetCustomerByIdQuery(NOT_EXISTING_CUSTOMER_ID);

            _customerStorageMock.Setup(storage => storage.GetByIdAsync(NOT_EXISTING_CUSTOMER_ID)).ReturnsAsync((Customer)null);

            // Act
            Func<Task<Customer>> act = () => _sender.Send(query);

            // Assert
            act.Should().ThrowAsync<CustomerNotFoundException>().WithMessage($"Customer with ID:{NOT_EXISTING_CUSTOMER_ID} doesn't exist");
        }

        [Fact]
        public async Task ShouldReturnCustomerWithTheSpecifiedId()
        {
            // Arrange
            const int CUSTOMER_ID = 5;
            var query = new GetCustomerByIdQuery(CUSTOMER_ID);

            _customerStorageMock.Setup(storage => storage.GetByIdAsync(CUSTOMER_ID)).ReturnsAsync(new Customer());

            // Act
            var customer = await _sender.Send(query);

            // Assert
            customer.Should().NotBeNull();
        }
    }
}
