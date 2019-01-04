using NUnit.Framework;
using HomesEngland.Gateway.Sql.Postgres;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class PostgresConnectionFactoryTests
    {
        private IDatabaseConnectionFactory _classUnderTest = null;
        public PostgresConnectionFactoryTests()
        {
            _classUnderTest = new PostgresDatabaseConnectionFactory(new PostgresDatabaseConnectionStringFormatter());
        }

        [Test]
        public void GivenValidConnectionString_ThenDoesntThrowExceptionWhenConnectingToDatabase()
        {
            //arrange 
            var connectionString = System.Environment.GetEnvironmentVariable("DATABASE_URL");
            //act
            var connection =_classUnderTest.Create(connectionString);
            //assert
            Assert.DoesNotThrow(()=>connection.Open());
        }

        [Test]
        public void GivenInValidConnectionString_ThenThrowsException()
        {
            //arrange 
            var connectionString = "postgres://username:password/host/dbName";
            //act
            //assert
            Assert.Throws<System.UriFormatException>(() => _classUnderTest.Create(connectionString));
        }
    }
}
