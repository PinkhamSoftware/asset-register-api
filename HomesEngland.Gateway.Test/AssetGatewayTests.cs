using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
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
    public class AssetGatewayTests
    {
        private readonly IGateway<IAsset, int> _classUnderTest;
        
        public AssetGatewayTests()
        {
            var assetRegisterConfiguration = ConfigurationHelper.GetAssetRegisterApiConfiguration(Directory.GetCurrentDirectory());
            var connectionString = assetRegisterConfiguration.ConnectionStrings.AssetRegisterApiDb;

            var assetGateway = new EFAssetGateway(connectionString);

            _classUnderTest = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(connectionString).Options);
            assetRegisterContext.Database.Migrate();
        }

        [Test]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame()
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();
                var createdAsset = await _classUnderTest.CreateAsync(entity).ConfigureAwait(false);
                //act
                var readAsset = await _classUnderTest.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                //assert
                readAsset.AssetIsEqual(createdAsset.Id, entity);
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
                var createdAsset = await _classUnderTest.CreateAsync(entity).ConfigureAwait(false);
                //act
                var readAsset = await _classUnderTest.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                //assert
                readAsset.AssetIsEqual(createdAsset.Id, entity);
                trans.Dispose();
            }
        }
    }
}
