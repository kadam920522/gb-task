using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GlobalBlue.CustomerManager.WebApi.IntegrationTests
{
    public class WebApiTestFixture : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly PostgresSqlTestcontainerDatabase _postgreSqlTestcontainer;
        private HttpClient _client;

        public WebApiTestFixture()
        {
            _factory = new WebApplicationFactory<Startup>();
            _postgreSqlTestcontainer = CreatePostgreSqlTestcontainerDatabase();
            Environment.SetEnvironmentVariable("DbConnectionString", _postgreSqlTestcontainer.ConnectionString);
        }

        public WebApplicationFactory<Startup> Factory => _factory;

        public string ConnectionString => _postgreSqlTestcontainer.ConnectionString;

        public HttpClient Client => _client;

        public async Task DisposeAsync()
        {
            Environment.SetEnvironmentVariable("DbConnectionString", string.Empty);
            await _postgreSqlTestcontainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlTestcontainer.StartAsync();
            _client = _factory.CreateClient();
        }

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
