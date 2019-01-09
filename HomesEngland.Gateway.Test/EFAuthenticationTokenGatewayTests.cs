using System;
using System.Threading.Tasks;
using System.Transactions;
using Bogus;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class EFAuthenticationTokenGatewayTests
    {
        private readonly IOneTimeAuthenticationTokenCreator _classUnderTest;

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
                    Email = faker.Internet.Email(),
                    Token = token,
                };
                //act
                var createdAuthenticationToken = await _classUnderTest.CreateAsync(authenticationToken).ConfigureAwait(false);
                //assert
                createdAuthenticationToken.Token.Should().BeEquivalentTo(authenticationToken.Token);
                createdAuthenticationToken.Expiry.Should().BeCloseTo(authenticationToken.Expiry);
                createdAuthenticationToken.Email.Should().Be(authenticationToken.Email);
                trans.Dispose();
            }
        }
    }
}