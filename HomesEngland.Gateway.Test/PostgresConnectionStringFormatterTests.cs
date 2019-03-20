using FluentAssertions;
/*using HomesEngland.Gateway.Sql.Postgres;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class PostgresConnectionStringFormatterTests
    {
        private readonly IDatabaseConnectionStringFormatter _classUnderTest;
        public PostgresConnectionStringFormatterTests()
        {
            _classUnderTest = new PostgresDatabaseConnectionStringFormatter();
        }

        [TestCase("postgres","db", "5432", "super-secret", "asset_register_api")]
        [TestCase("test", "localhost", "15432", "super-not-secret", "local_db")]
        public void GivenValidConnectionString_ThenReturnsConnection(string userId, string server, string port, string password, string database)
        {
            //arrange 
            var databaseUrl = $"postgres://{userId}:{password}@{server}:{port}/{database}";
            //act
            var connection = _classUnderTest.BuildConnectionStringFromUrl(databaseUrl);
            //assert
            connection.Should().BeEquivalentTo($"Server={server};Port={port};User Id={userId};Password={password};Database={database};SSL Mode=Prefer;");
        }
    }
}*/
