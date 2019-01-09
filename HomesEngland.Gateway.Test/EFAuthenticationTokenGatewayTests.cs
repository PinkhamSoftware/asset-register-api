using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Bogus;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class EFAuthenticationTokenGatewayTests
    {
        private readonly IAuthenticationGateway _classUnderTest;

        public EFAuthenticationTokenGatewayTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var gateway = new EFAuthenticationTokenGateway(databaseUrl);

            _classUnderTest = gateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("secure",5)]
        [TestCase("token",6)]
        [TestCase("test", 7)]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame(string token, int seconds)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var faker = new Faker();
                IAuthenticationToken authenticationToken = new AuthenticationToken
                {
                    Expiry = DateTime.UtcNow.AddSeconds(seconds),
                    ReferenceNumber = faker.UniqueIndex.ToString(),
                    Token = token,
                };
                //act
                var createdAuthenticationToken = await _classUnderTest.CreateAsync(authenticationToken, CancellationToken.None).ConfigureAwait(false);
                var readAuthenticationToken = await _classUnderTest.ReadAsync(createdAuthenticationToken.Id, CancellationToken.None).ConfigureAwait(false);
                //assert
                readAuthenticationToken.Token.Should().BeEquivalentTo(authenticationToken.Token);
                readAuthenticationToken.Expiry.Should().BeCloseTo(authenticationToken.Expiry);
                readAuthenticationToken.ReferenceNumber.Should().Be(authenticationToken.ReferenceNumber);
                trans.Dispose();
            }
        }
    }
}
