using DotNet.Testcontainers.Containers.Configurations;
using DotNet.Testcontainers.Containers.Modules.Abstractions;

namespace GlobalBlue.CustomerManager.WebApi.IntegrationTests
{
    public sealed class PostgresSqlTestcontainerDatabase : TestcontainerDatabase
    {
        internal PostgresSqlTestcontainerDatabase(ITestcontainersConfiguration configuration) : base(configuration)
        {
        }

        public override string ConnectionString => "Server=localhost;Port=5433;Database=db;User Id=postgres;Password=postgres;";
    }
}
