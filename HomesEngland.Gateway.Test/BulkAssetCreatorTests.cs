using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class BulkAssetCreatorTests
    {
        private readonly IGateway<IAsset, int> _gateway;
        private readonly IBulkAssetCreator _classUnderTest;

        public BulkAssetCreatorTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);
            _classUnderTest = assetGateway;
            _gateway = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenAnAssetHasBeenCreated_WhenTheAssetIsReadFromTheGateway_ThenItIsTheSame(int count)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                
                IList<IAsset> assets = new List<IAsset>();
                for (int i = 0; i < count; i++)
                {
                    var entity = TestData.Domain.GenerateAsset();
                    assets.Add(entity);
                }
                
                var createdAssets = await _classUnderTest.BulkCreateAsync(assets, CancellationToken.None).ConfigureAwait(false);
                //act
                for (int i = 0; i < count; i++)
                {
                    var createdAsset = createdAssets.ElementAtOrDefault(i);
                    var readAsset = await _gateway.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                    //assert
                    createdAsset.Id.Should().NotBe(0);
                    readAsset.AssetIsEqual(createdAsset.Id, readAsset);
                }
                
                trans.Dispose();
            }
        }
    }
}