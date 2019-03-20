using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Bogus;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class EFAuthenticationTokenGatewayTests
    {
        private readonly IAuthenticationGateway _classUnderTest;

        public EFAuthenticationTokenGatewayTests()
        {
            var assetRegisterConfiguration = ConfigurationHelper.GetAssetRegisterApiConfiguration(Directory.GetCurrentDirectory());
            var connectionString = assetRegisterConfiguration.ConnectionStrings.AssetRegisterApiDb;

            var gateway = new EFAuthenticationTokenGateway(connectionString);

            _classUnderTest = gateway;

            var assetRegisterContext = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(connectionString).Options);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("test@test.com", "secure", 5)]
        [TestCase("meow@cat.com", "token", 6)]
        [TestCase("woof@dog.com", "test", 7)]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame(string email,
            string token,
            int seconds)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Faker faker = new Faker();
                IAuthenticationToken authenticationToken = new AuthenticationToken
                {
                    Expiry = DateTime.UtcNow.AddSeconds(seconds),
                    ReferenceNumber = faker.UniqueIndex.ToString(),
                    Token = token,
                    EmailAddress = email
                };

                //act
                IAuthenticationToken createdAuthenticationToken = await _classUnderTest
                    .CreateAsync(authenticationToken, CancellationToken.None).ConfigureAwait(false);
                IAuthenticationToken readAuthenticationToken = await _classUnderTest
                    .ReadAsync(createdAuthenticationToken.Token, CancellationToken.None).ConfigureAwait(false);
                //assert
                readAuthenticationToken.EmailAddress.Should().BeEquivalentTo(email);
                readAuthenticationToken.Token.Should().BeEquivalentTo(authenticationToken.Token);
                readAuthenticationToken.Expiry.Should().BeCloseTo(authenticationToken.Expiry);
                readAuthenticationToken.ReferenceNumber.Should().Be(authenticationToken.ReferenceNumber);
            }
        }

        [TestCase("secure")]
        [TestCase("token")]
        [TestCase("test")]
        public async Task GivenAnAuthenticationTokenHasBeenCreated_WhenItIsDeleted_ItCanNoLongerBeRead(string token)
        {
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                IAuthenticationToken authenticationToken = new AuthenticationToken
                {
                    Token = token,
                    Expiry = DateTime.UtcNow,
                    ReferenceNumber = "RefNumber"
                };

                await _classUnderTest.CreateAsync(authenticationToken, CancellationToken.None).ConfigureAwait(false);
                await _classUnderTest.DeleteAsync(token, CancellationToken.None).ConfigureAwait(false);

                var foundToken = await _classUnderTest.ReadAsync(token, CancellationToken.None)
                    .ConfigureAwait(false);

                foundToken.Should().BeNull();
            }
        }
    }
}
