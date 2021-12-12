using FluentAssertions;
using FluentValidation;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using GlobalBlue.CustomerManager.Application.Update;
using MediatR;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.Application.UnitTests.Update
{
    public sealed class UpdateCustomerCommandTest : ApplicationTestBase
    {
        private const int CUSTOMER_ID = 5;
        private const string ETAG = "643244321";
        private const string NEW_FIRST_NAME = "FirstName";
        private const string NEW_SURNAME = "Surname";
        private const string NEW_EMAIL_ADDRESS = "email@domain.com";
        private const string NEW_PASSWORD = "password";
        private const string ORIGINAL_EMAIL_ADDRESS = "original@domain.com";

        [Fact]
        public void ShouldThrowCustomerNotFoundException_WhenNoSuchCustomerWithTheSpecifiedId()
        {
            // Arrange
            const int NOT_EXISTING_CUSTOMER_ID = 5;

            var command = new UpdateCustomerCommand(ETAG, NOT_EXISTING_CUSTOMER_ID, NEW_FIRST_NAME, NEW_SURNAME, NEW_EMAIL_ADDRESS, NEW_PASSWORD);

            _customerStorageMock.Setup(storage => storage.GetByIdAsync(NOT_EXISTING_CUSTOMER_ID)).ReturnsAsync((Customer)null);

            // Act
            Func<Task<Unit>> act = () => _sender.Send(command);

            // Assert
            act.Should().ThrowAsync<CustomerNotFoundException>().WithMessage($"Customer with ID:{NOT_EXISTING_CUSTOMER_ID} doesn't exist");
        }

        [Fact]
        public void ShouldThrowCustomerEmailAddressConflicException_WhenThereIsAnAlreadyExistingCustomerWithTheNewEmailAddress()
        {
            // Arrange
            var customerToBeUpdated = new Customer { Id = CUSTOMER_ID, EmailAddress = ORIGINAL_EMAIL_ADDRESS };
            _customerStorageMock.Setup(storage => storage.GetByIdAsync(CUSTOMER_ID)).ReturnsAsync(customerToBeUpdated);

            var customerExistingWithTheEmailAddress = new Customer();
            _customerStorageMock.Setup(storage => storage.GetByEmailAddressAsync(NEW_EMAIL_ADDRESS)).ReturnsAsync(customerExistingWithTheEmailAddress);

            var command = new UpdateCustomerCommand(ETAG, CUSTOMER_ID, NEW_FIRST_NAME, NEW_SURNAME, NEW_EMAIL_ADDRESS, NEW_PASSWORD);

            // Act
            Func<Task> act = () => _sender.Send(command);

            // Assert
            act.Should().ThrowAsync<CustomerEmailAddressConflictException>().WithMessage($"Customer already exists with the following e-mail address: {NEW_EMAIL_ADDRESS}");
        }

        [Fact]
        public async Task ShouldUpdateTheCustomer_WhenThereIsNoConflict()
        {
            // Arrange
            const string HASHED_PASSWORD = "hashed_password";

            var customerToBeUpdated = new Customer { Id = CUSTOMER_ID, EmailAddress = ORIGINAL_EMAIL_ADDRESS };
            _customerStorageMock.Setup(storage => storage.GetByIdAsync(CUSTOMER_ID)).ReturnsAsync(customerToBeUpdated);

            var customerExistingWithTheEmailAddress = new Customer();
            _customerStorageMock.Setup(storage => storage.GetByEmailAddressAsync(NEW_EMAIL_ADDRESS)).ReturnsAsync((Customer)null);

            _passwordHasherMock.Setup(hasher => hasher.Hash(NEW_PASSWORD)).Returns(HASHED_PASSWORD);

            var command = new UpdateCustomerCommand(ETAG, CUSTOMER_ID, NEW_FIRST_NAME, NEW_SURNAME, NEW_EMAIL_ADDRESS, NEW_PASSWORD);

            var expectedCustomer = new Customer 
            { 
                Id = CUSTOMER_ID, 
                EmailAddress = NEW_EMAIL_ADDRESS, 
                FirstName = NEW_FIRST_NAME, 
                Surname = NEW_SURNAME, 
                Password = HASHED_PASSWORD 
            };

            // Act
            await _sender.Send(command);

            // Assert
            _customerStorageMock.Verify(storage => storage.UpdateAsync(It.Is<Customer>(customer => ShouldBeEquivalent(customer, expectedCustomer))), Times.Once);
        }

        [Theory]
        [InlineData(null, NEW_SURNAME, NEW_EMAIL_ADDRESS, NEW_PASSWORD)]
        [InlineData("", NEW_SURNAME, NEW_EMAIL_ADDRESS, NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, null, NEW_EMAIL_ADDRESS, NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, "", NEW_EMAIL_ADDRESS, NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, NEW_SURNAME, null, NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, NEW_SURNAME, "", NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, NEW_SURNAME, "invalidEmailAddress", NEW_PASSWORD)]
        [InlineData(NEW_FIRST_NAME, NEW_SURNAME, NEW_EMAIL_ADDRESS, null)]
        [InlineData(NEW_FIRST_NAME, NEW_SURNAME, NEW_EMAIL_ADDRESS, "")]
        public void ShouldThrowValidationException_WhenOneOfRequiredFieldIsNullOrEmpty(string firstName, string surname, string emailAddress, string password)
        {
            // Arrange
            var invalidCommand = new UpdateCustomerCommand(ETAG, customerId: It.IsAny<int>(), firstName, surname, emailAddress, password);

            // Act
            Func<Task> act = () => _sender.Send(invalidCommand);

            // Assert
            act.Should().ThrowAsync<ValidationException>();
        }

        private static bool ShouldBeEquivalent(Customer customer, Customer expectedCustomer)
        {
            customer.Should().BeEquivalentTo(expectedCustomer);
            return true;
        }
    }
}
