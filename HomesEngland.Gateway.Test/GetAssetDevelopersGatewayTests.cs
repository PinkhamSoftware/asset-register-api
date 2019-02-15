using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets.Developer;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class GetAssetDevelopersGatewayTests
    {
        private readonly IAssetDeveloperLister _classUnderTest;
        private readonly IGateway<IAsset, int> _gateway;

        public GetAssetDevelopersGatewayTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);

            _gateway = assetGateway;
            _classUnderTest = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("Developer 1")]
        [TestCase("Developer 2")]
        [TestCase("Developer 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame(string developer)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();
                entity.DevelopingRslName = developer;
                await _gateway.CreateAsync(entity).ConfigureAwait(false);
                //act
                var developers = await _classUnderTest.ListDevelopersAsync(CancellationToken.None).ConfigureAwait(false);
                //assert
                developers.Count.Should().Be(1);
                trans.Dispose();
            }
        }
    }
}