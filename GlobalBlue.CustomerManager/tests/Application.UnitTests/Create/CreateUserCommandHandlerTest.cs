using FluentAssertions;
using GlobalBlue.CustomerManager.Application.Common.Abstract;
using GlobalBlue.CustomerManager.Application.Create;
using GlobalBlue.CustomerManager.Application.Entites;
using GlobalBlue.CustomerManager.Application.Exceptions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.Application.UnitTests.Create
{
    public sealed class CreateUserCommandHandlerTest
    {
        private readonly CreateCustomerCommandHandler _sut;

        private readonly Mock<ICustomerStorage> _customerStorageMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public CreateUserCommandHandlerTest()
        {
            _customerStorageMock = new Mock<ICustomerStorage>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _sut = new CreateCustomerCommandHandler(_customerStorageMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowCustomerConflictException_WhenCustomerExistsWithTheSpecifiedEmailAddress()
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
            Func<Task<int>> act = () => _sut.Handle(command, It.IsAny<CancellationToken>());

            // Assert
            act.Should().ThrowAsync<CustomerConflictException>().WithMessage($"Customer already exists with the following e-mail address: {EXISTING_EMAIL_ADDRESS}");
        }

        [Fact]
        public async Task Handle_ShouldReturnTheNewlyCreatedCustomersId_WhenNoCustomerExistsWithTheSpecifiedEmailAddress()
        {
            // Arrange
            const string NON_EXISTING_EMAIL_ADDRESS = "nonexisting@email.com";
            const string PASSWORD = "password";
            const string HASHED_PASSWORD = "hashedpass";

            var command = new CreateCustomerCommand(
                firstName: It.IsAny<string>(),
                surname: It.IsAny<string>(),
                NON_EXISTING_EMAIL_ADDRESS,
                PASSWORD);

            const int NEWLY_CREATED_CUSTOMER_ID = 13;

            _customerStorageMock.Setup(storage => storage.GetByEmailAddressAsync(NON_EXISTING_EMAIL_ADDRESS)).ReturnsAsync(default(Customer));
            _passwordHasherMock.Setup(hasher => hasher.Hash(PASSWORD)).Returns(HASHED_PASSWORD);
            _customerStorageMock
                .Setup(storage => storage.AddAsync(It.Is<Customer>(customer => ShouldBe(customer, NON_EXISTING_EMAIL_ADDRESS, HASHED_PASSWORD))))
                .ReturnsAsync(NEWLY_CREATED_CUSTOMER_ID);

            // Act
            var customerId = await _sut.Handle(command, It.IsAny<CancellationToken>());

            // Assert
            customerId.Should().Be(NEWLY_CREATED_CUSTOMER_ID);
        }

        private static bool ShouldBe(Customer customer, string emailAddress, string hashedPassword) =>
            customer.EmailAddress == emailAddress && customer.Password == hashedPassword;
    }
}
