using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using FluentAssertions;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using CustomerRequestDto = GlobalBlue.CustomerManager.WebApi.DataTransferObjects.Request.CustomerDto;
using CustomerResponseDto = GlobalBlue.CustomerManager.WebApi.DataTransferObjects.Response.CustomerDto;

namespace GlobalBlue.CustomerManager.WebApi.IntegrationTests.Controllers
{
    public class CustomersControllerTest : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly PostgresSqlTestcontainerDatabase _postgreSqlTestcontainer;

        public CustomersControllerTest(WebApplicationFactory<Startup> factory)
        {
            _postgreSqlTestcontainer = CreatePostgreSqlTestcontainerDatabase();
            Environment.SetEnvironmentVariable("DbConnectionString", _postgreSqlTestcontainer.ConnectionString);
            _factory = factory;
        }

        public async Task DisposeAsync()
        {
            Environment.SetEnvironmentVariable("DbConnectionString", string.Empty);
            await _postgreSqlTestcontainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlTestcontainer.StartAsync();
        }

        [Fact]
        public async Task GetAllCustomers_Test()
        {
            // Arrange
            var client = _factory.CreateClient();
            var testCustomers = GetTestCustomers();
            await SetupTestCustomers(testCustomers);
            var expectedCustomers = testCustomers.Select(customer => CustomerResponseDto.MapFrom(customer));

            // Act
            var response = await client.GetAsync("api/customers");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var responseCustomers = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerResponseDto>>();
            responseCustomers.Should().BeEquivalentTo(expectedCustomers, options => options.WithoutStrictOrdering());
        }

        [Fact]
        public async Task GetCustomerById_Test()
        {
            // Arrange
            const int CUSTOMER_ID = 3;
            const string EMAIL_ADDRESS = "test3@domain.com";
            const string FIRST_NAME = "Jose";
            const string SURNAME = "DeMarco";
            const string PASSWORD = "hashedpassword";

            var testCustomers = GetTestCustomers().ToList();
            testCustomers.Add(new Customer { EmailAddress = EMAIL_ADDRESS, FirstName = FIRST_NAME, Surname = SURNAME, Password = PASSWORD });

            var client = _factory.CreateClient();
            
            await SetupTestCustomers(testCustomers);
            var expectedCustomer = new CustomerResponseDto { Id = CUSTOMER_ID, EmailAddress = EMAIL_ADDRESS, Password = PASSWORD, FirstName = FIRST_NAME, Surname = SURNAME };

            // Act
            var response = await client.GetAsync($"api/customers/{CUSTOMER_ID}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var responseCustomer = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
            responseCustomer.Should().BeEquivalentTo(expectedCustomer);
        }

        [Fact]
        public async Task CreateCustomer_Test()
        {
            // Arrange
            const string EMAIL_ADDRESS = "test3@domain.com";
            const string FIRST_NAME = "Jose";
            const string SURNAME = "DeMarco";
            const string PASSWORD = "hashedpassword";

            var createCustomerRequestBody = new CustomerRequestDto { EmailAddress = EMAIL_ADDRESS, FirstName = FIRST_NAME, Surname = SURNAME, Password = PASSWORD };
            var expectedCustomer = new CustomerResponseDto { Id = 1, EmailAddress = EMAIL_ADDRESS, FirstName = FIRST_NAME, Surname = SURNAME };

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync($"api/customers", JsonContent.Create(createCustomerRequestBody));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var responseCustomer = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
            responseCustomer.Should().BeEquivalentTo(expectedCustomer, options => options.Excluding(c => c.Password));
            responseCustomer.Password.Should().NotBeNullOrWhiteSpace();
            response.Headers.Location.ToString().ToLower().Should().EndWith("api/customers/1");
        }

        [Fact]
        public async Task UpdateCustomer_Test()
        {
            // Arrange
            const string NEW_EMAIL_ADDRESS = "test3@domain.com";
            const string NEW_FIRST_NAME = "Jose";
            const string NEW_SURNAME = "DeMarco";
            const string NEW_PASSWORD = "hashedpassword";

            var updateCustomerRequestBody = new CustomerRequestDto { EmailAddress = NEW_EMAIL_ADDRESS, FirstName = NEW_FIRST_NAME, Surname = NEW_SURNAME, Password = NEW_PASSWORD };

            var client = _factory.CreateClient();

            var originalCustomer = new Customer { EmailAddress = "original@domain.com", FirstName = "original", Surname = "original", Password = "original_hashed" };
            await SetupTestCustomers(new[] { originalCustomer });

            var expectedCustomerResponse = new CustomerResponseDto { Id = originalCustomer.Id, EmailAddress = NEW_EMAIL_ADDRESS, FirstName = NEW_FIRST_NAME, Surname = NEW_SURNAME };

            // Act
            var response = await client.PutAsync($"api/customers/{originalCustomer.Id}", JsonContent.Create(updateCustomerRequestBody));
            var updatedCustomerResponse = await GetUpdatedCustomer(client, originalCustomer.Id);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            updatedCustomerResponse.Should().BeEquivalentTo(expectedCustomerResponse, options => options.Excluding(c => c.Password));
            updatedCustomerResponse.Password.Should().NotBeNullOrWhiteSpace();
        }

        private async Task<CustomerResponseDto> GetUpdatedCustomer(HttpClient client, int customerId)
        {
            var response = await client.GetAsync($"api/customers/{customerId}");
            return await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        }

        private async Task SetupTestCustomers(IEnumerable<Customer> customers)
        {
            using var serviceScope = _factory.Services.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<CustomerManagerDbContext>();
            dbContext.Customers.AddRange(customers);
            await dbContext.SaveChangesAsync();
        }

        private IEnumerable<Customer> GetTestCustomers() => new[]
            {
                new Customer{EmailAddress = "test1@domain.com", FirstName = "Joe", Surname = "Doe", Password = "hashedpassw"},
                new Customer{EmailAddress = "test2@domain.com", FirstName = "Jane", Surname = "Doe", Password = "hashedpassw"}
            };

        private PostgresSqlTestcontainerDatabase CreatePostgreSqlTestcontainerDatabase() =>
            new TestcontainersBuilder<PostgresSqlTestcontainerDatabase>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres",
            })
            .WithPortBinding(5433, 5432)
            .Build();
    }
}
