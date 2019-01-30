using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class BulkAssetCreatorTests
    {
        private readonly IGateway<IAsset, int> _gateway;
        private readonly IAssetRegisterVersionCreator _classUnderTest;

        public BulkAssetCreatorTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);
            _classUnderTest = new EFAssetRegisterVersionGateway(databaseUrl);
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
                
                var createdAssets = await _classUnderTest.CreateAsync(new AssetRegisterVersion
                {
                    Assets = assets
                }, CancellationToken.None).ConfigureAwait(false);
                //act
                for (int i = 0; i < count; i++)
                {
                    var createdAsset = createdAssets.Assets.ElementAtOrDefault(i);
                    var readAsset = await _gateway.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                    //assert
                    createdAsset.Id.Should().NotBe(0);
                    readAsset.AssetIsEqual(createdAsset.Id, readAsset);
                }
                
                trans.Dispose();
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenEntities_WhenImporting_ThenAndAssetRegisterVersionIsCreated(int count)
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

                var createdAssets = await _classUnderTest.CreateAsync(new AssetRegisterVersion
                {
                    Assets = assets,
                }, CancellationToken.None).ConfigureAwait(false);
                //act
                for (int i = 0; i < count; i++)
                {
                    var createdAsset = createdAssets.Assets.ElementAtOrDefault(i);
                    var readAsset = await _gateway.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                    //assert
                    readAsset.AssetRegisterVersionId.Should().NotBeNull();
                }

                trans.Dispose();
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenEntities_WhenImporting_ThenAndAssetRegisterVersionIsTimeStamped(int count)
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

                var timeStamp = DateTime.UtcNow;
                var assetRegisterVersion = await _classUnderTest.CreateAsync(new AssetRegisterVersion
                {
                    Assets = assets,
                    ModifiedDateTime = timeStamp
                }, CancellationToken.None).ConfigureAwait(false);
                //act
                for (int i = 0; i < count; i++)
                {
                    var createdAsset = assetRegisterVersion.Assets.ElementAtOrDefault(i);
                    var readAsset = await _gateway.ReadAsync(createdAsset.Id).ConfigureAwait(false);
                    //assert
                    readAsset.AssetRegisterVersionId.Should().NotBeNull();
                }

                trans.Dispose();
            }
        }
    }
}
