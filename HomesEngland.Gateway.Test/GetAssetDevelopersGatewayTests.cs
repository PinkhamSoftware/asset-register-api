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

        [TestCase("Developer 1", 2)]
        [TestCase("Developer 2", 3)]
        [TestCase("Developer 3", 4)]
        public async Task GivenThatMultipleAssetsHaveBeenCreatedWithIdenticalDevelopers_WhenWeListDevelopers_ThenWeGetTheUniqueDevelopers(string developer, int count)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                for (int i = 0; i < count; i++)
                {
                    await CreateAsset(developer);    
                }
                //act
                var developers = await _classUnderTest.ListDevelopersAsync(CancellationToken.None).ConfigureAwait(false);
                //assert
                developers.Count.Should().Be(1);
                trans.Dispose();
            }
        }

        [TestCase("Developer 1", 2)]
        [TestCase("Developer 2", 3)]
        [TestCase("Developer 3", 4)]
        public async Task GivenThatMultipleAssetsHaveBeenCreatedWithDifferentDevelopers_WhenWeListDevelopers_ThenItReturnsTheExpectedNumberOfDevelopers(string developer, int count)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                for (int i = 0; i < count; i++)
                {
                    await CreateAsset(developer + i);
                }
                
                //act
                var developers = await _classUnderTest.ListDevelopersAsync(CancellationToken.None).ConfigureAwait(false);
                //assert
                developers.Count.Should().Be(count);
                trans.Dispose();
            }
        }

        [TestCase("Developer 1")]
        [TestCase("Developer 2")]
        [TestCase("Developer 3")]
        public async Task GivenThatNoAssetsHaveBeenCreated_WhenWeListDevelopers_ThenWeGetNoResults(string developer)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //act
                var developers = await _classUnderTest.ListDevelopersAsync(CancellationToken.None).ConfigureAwait(false);
                //assert
                developers.Count.Should().Be(0);
                trans.Dispose();
            }
        }

        private async Task CreateAsset(string developer)
        {
            var entity = TestData.Domain.GenerateAsset();
            entity.DevelopingRslName = developer;
            await _gateway.CreateAsync(entity).ConfigureAwait(false);
        }
    }
}
