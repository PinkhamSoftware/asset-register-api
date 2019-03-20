using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Assets.Region;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class GetAssetRegionsGatewayTests
    {
        private readonly IAssetRegionLister _classUnderTest;
        private readonly IGateway<IAsset, int> _gateway;

        public GetAssetRegionsGatewayTests()
        {
            var assetRegisterConfiguration = ConfigurationHelper.GetAssetRegisterApiConfiguration(Directory.GetCurrentDirectory());
            var connectionString = assetRegisterConfiguration.ConnectionStrings.AssetRegisterApiDb;

            var assetGateway = new EFAssetGateway(connectionString);

            _gateway = assetGateway;
            _classUnderTest = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(connectionString).Options);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("Region 1")]
        [TestCase("Region 2")]
        [TestCase("Region 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame(string region)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();
                entity.ImsOldRegion = region;
                entity.LocationLaRegionName = region;
                var createdAsset = await _gateway.CreateAsync(entity).ConfigureAwait(false);
                //act
                var regions = await _classUnderTest.ListRegionsAsync(CancellationToken.None).ConfigureAwait(false);
                //assert
                regions.Count.Should().Be(1);
                trans.Dispose();
            }
        }

        [Test]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenTheAssetsAreReadFromTheGateway_ThenItIsTheSame()
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();
                var createdAsset = await _gateway.CreateAsync(entity).ConfigureAwait(false);
                //act
                var readAsset = await _gateway.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                //assert
                readAsset.AssetIsEqual(createdAsset.Id, entity);
                trans.Dispose();
            }
        }
    }
}
