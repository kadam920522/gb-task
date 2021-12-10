using FluentAssertions;
using FluentValidation;
using GlobalBlue.CustomerManager.Application.Create;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.Application.UnitTests.Create
{
    public sealed class CreateUserCommandTest : ApplicationTestBase
    {
        private const string FIRST_NAME = "FirstName";
        private const string SURNAME = "Surname";
        private const string EMAIL_ADDRESS = "email@domain.com";
        private const string PASSWORD = "password";

        [Fact]
        public void ShouldThrowCustomerConflictException_WhenCustomerExistsWithTheSpecifiedEmailAddress()
        {
            // Arrange
            const string EXISTING_EMAIL_ADDRESS = "existing@email.com";
            var command = new CreateCustomerCommand(
                firstName: It.IsAny<string>(),
                surname: It.IsAny<string>(),
                EXISTING_EMAIL_ADDRESS,
                password: It.IsAny<string>());

            _customerStorageMock.Setup(storage => storage.GetByEmailAddressAsync(EXISTING_EMAIL_ADDRESS)).ReturnsAsync(new Customer());

            // Act
            Func<Task<Customer>> act = () => _sender.Send(command);

            // Assert
            act.Should().ThrowAsync<CustomerConflictException>().WithMessage($"Customer already exists with the following e-mail address: {EXISTING_EMAIL_ADDRESS}");
        }

        [Fact]
        public async Task ShouldReturnTheNewlyCreatedCustomer_WhenNoCustomerExistsWithTheSpecifiedEmailAddress()
        {
            // Arrange
            const string HASHED_PASSWORD = "hashedpass";

            var command = new CreateCustomerCommand(
                FIRST_NAME,
                SURNAME,
                EMAIL_ADDRESS,
                PASSWORD);

            var expectedCustomer = new Customer 
            { 
                Id = 13,
                EmailAddress = EMAIL_ADDRESS,
                FirstName = FIRST_NAME,
                Surname = SURNAME, 
                Password = HASHED_PASSWORD 
            };

            _customerStorageMock.Setup(storage => storage.GetByEmailAddressAsync(EMAIL_ADDRESS)).ReturnsAsync(default(Customer));
            _passwordHasherMock.Setup(hasher => hasher.Hash(PASSWORD)).Returns(HASHED_PASSWORD);
            _customerStorageMock
                .Setup(storage => storage.AddAsync(It.Is<Customer>(customer => ShouldBeEquivalent(customer, expectedCustomer))))
                .ReturnsAsync(expectedCustomer);

            // Act
            var newCustomer = await _sender.Send(command);

            // Assert
            newCustomer.Should().BeEquivalentTo(expectedCustomer);
        }

        [Theory]
        [InlineData(null, SURNAME, EMAIL_ADDRESS, PASSWORD)]
        [InlineData("", SURNAME, EMAIL_ADDRESS, PASSWORD)]
        [InlineData(FIRST_NAME, null, EMAIL_ADDRESS, PASSWORD)]
        [InlineData(FIRST_NAME, "", EMAIL_ADDRESS, PASSWORD)]
        [InlineData(FIRST_NAME, SURNAME, null, PASSWORD)]
        [InlineData(FIRST_NAME, SURNAME, "", PASSWORD)]
        [InlineData(FIRST_NAME, SURNAME, EMAIL_ADDRESS, null)]
        [InlineData(FIRST_NAME, SURNAME, EMAIL_ADDRESS, "")]
        public void ShouldThrowValidationException_WhenOneOfRequiredFieldIsNullOrEmpty(string firstName, string surname, string emailAddress, string password)
        {
            // Arrange
            var invalidCommand = new CreateCustomerCommand(firstName, surname, emailAddress, password);

            // Act
            Func<Task<Customer>> act = () => _sender.Send(invalidCommand);

            // Assert
            act.Should().ThrowAsync<ValidationException>();
        }

        private static bool ShouldBeEquivalent(Customer customer, Customer expectedCustomer)
        {
            customer.Should().BeEquivalentTo(expectedCustomer, options => options.Excluding(c => c.Id));
            return true;
        }
    }
}
