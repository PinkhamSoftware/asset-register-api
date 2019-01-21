using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.BulkCreateAsset.Models;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class AssetAggregatorGatewayTests
    {
        private readonly IAssetAggregator _classUnderTest;
        private readonly IGateway<IAsset, int> _gateway;
        private readonly IBulkAssetCreator _bulkAssetCreator;

        public AssetAggregatorGatewayTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);

            _gateway = assetGateway;
            _bulkAssetCreator = assetGateway;

            _classUnderTest = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase(2, 1, 1001, null,null)]
        [TestCase(2, 1, 2002, null,null)]
        [TestCase(2, 1, 3003, null,null)]
        [TestCase(2, 2, null, "test unique", "test")]
        [TestCase(2, 2, null, "Test unique", "test")]
        [TestCase(2, 2, null, "uniiique"   , "unii")]
        [TestCase(2, 2, null, "testing 4"  , "tes")]
        [TestCase(2, 2, null, "Testing 4"  , "tes")]
        [TestCase(2, 2, null, "test uniQue", "que")]
        [TestCase(2, 2, null, "uniiique", null)]
        [TestCase(2, 2, null, " ", null)]
        [TestCase(2, 2, null, "", null)]
        [TestCase(2, 2, null, " ", " ")]
        [TestCase(2, 2, null, "", "")]
        [TestCase(2, 2, null, null, null)]
        [TestCase(2, 0, null, "testing 4", "3")]
        [TestCase(2, 0, null, "testing 4", "1")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateViaSearchCritera_ThenWeCanFindHowManyUniqueRecordsThereAre(int count, int expectedCount, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assetRegisterVersionId = await CreateAggregatedAssets(count, schemeId, address, null, null);
                
                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersionId
                };
                //act
                var assets = await _classUnderTest.Aggregate(aggregatedSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.UniqueRecords.Should().Be(expectedCount);
                trans.Dispose();
            }
        }

        [TestCase(2, 2, 1000.01, 1001, null, null)]
        [TestCase(2, 2, 1000.01, 2002, null, null)]
        [TestCase(2, 2, 1000.01, 3003, null, null)]
        [TestCase(2, 2, 1000.01, null, "test unique", "test")]
        [TestCase(2, 2, 1000.01, null, "Test unique", "test")]
        [TestCase(2, 2, 1000.01, null, "uniiique", "unii")]
        [TestCase(2, 2, 1000.01, null, "testing 4", "tes")]
        [TestCase(2, 2, 1000.01, null, "Testing 4", "tes")]
        [TestCase(2, 2, 1000.01, null, "test uniQue", "que")]
        [TestCase(2, 2, 1000.01, null, "uniiique", null)]
        [TestCase(2, 2, 1000.01, null, " ", null)]
        [TestCase(2, 2, 1000.01, null, "", null)]
        [TestCase(2, 2, 1000.01, null, " ", " ")]
        [TestCase(2, 2, 1000.01, null, "", "")]
        [TestCase(2, 2, 1000.01, null, null, null)]
        [TestCase(2, 0, 1000.01, null, "testing 4", "3")]
        [TestCase(2, 0, 1000.01, null, "testing 4", "1")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateViaSearchCritera_ThenWeCanGetAssetValue(int count, int expectedCount, decimal? agencyFairValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assetRegisterVersionId = await CreateAggregatedAssets(count, schemeId, address, agencyFairValue,null);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersionId,
                };
                //act
                var assets = await _classUnderTest.Aggregate(aggregatedSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.AssetValue.Should().Be(agencyFairValue*expectedCount);
                trans.Dispose();
            }
        }

        [TestCase(2, 2, 1000.01, 1001, null, null)]
        [TestCase(2, 2, 1000.01, 2002, null, null)]
        [TestCase(2, 2, 1000.01, 3003, null, null)]
        [TestCase(2, 2, 1000.01, null, "test unique", "test")]
        [TestCase(2, 2, 1000.01, null, "Test unique", "test")]
        [TestCase(2, 2, 1000.01, null, "uniiique", "unii")]
        [TestCase(2, 2, 1000.01, null, "testing 4", "tes")]
        [TestCase(2, 2, 1000.01, null, "Testing 4", "tes")]
        [TestCase(2, 2, 1000.01, null, "test uniQue", "que")]
        [TestCase(2, 2, 1000.01, null, "uniiique", null)]
        [TestCase(2, 2, 1000.01, null, " ", null)]
        [TestCase(2, 2, 1000.01, null, "", null)]
        [TestCase(2, 2, 1000.01, null, " ", " ")]
        [TestCase(2, 2, 1000.01, null, "", "")]
        [TestCase(2, 2, 1000.01, null, null, null)]
        [TestCase(2, 0, 1000.01, null, "testing 4", "3")]
        [TestCase(2, 0, 1000.01, null, "testing 4", "1")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateViaSearchCritera_ThenWeCanGetMoneyPaidOut(int count, int expectedCount, decimal? agencyEquityValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assetRegisterVersionId = await CreateAggregatedAssets(count, schemeId, address, null, agencyEquityValue);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersionId
                };
                //act
                var assets = await _classUnderTest.Aggregate(aggregatedSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.MoneyPaidOut.Should().Be(agencyEquityValue * expectedCount);
                trans.Dispose();
            }
        }

        [TestCase(2, 2, 1000.01, 301.301, 1001, null, null)]
        [TestCase(2, 2, 1000.01, 301.301, 2002, null, null)]
        [TestCase(2, 2, 1000.01, 301.301, 3003, null, null)]
        [TestCase(2, 2, 1000.01, 301.301, null, "test unique", "test")]
        [TestCase(2, 2, 1000.01, 301.301, null, "Test unique", "test")]
        [TestCase(2, 2, 1000.01, 301.301, null, "uniiique", "unii")]
        [TestCase(2, 2, 1000.01, 301.301, null, "testing 4", "tes")]
        [TestCase(2, 2, 1000.01, 301.301, null, "Testing 4", "tes")]
        [TestCase(2, 2, 1000.01, 301.301, null, "test uniQue", "que")]
        [TestCase(2, 2, 1000.01, 301.301, null, "uniiique", null)]
        [TestCase(2, 2, 1000.01, 301.301, null, " ", null)]
        [TestCase(2, 2, 1000.01, 301.301, null, "", null)]
        [TestCase(2, 2, 1000.01, 301.301, null, " ", " ")]
        [TestCase(2, 2, 1000.01, 301.301, null, "", "")]
        [TestCase(2, 2, 1000.01, 301.301, null, null, null)]
        [TestCase(2, 0, 1000.01, 301.301, null, "testing 4", "3")]
        [TestCase(2, 0, 1000.01, 301.301, null, "testing 4", "1")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateViaSearchCritera_ThenWeCanGetMovementInFairValue(int count, int expectedCount, decimal? agencyFairValue, decimal? agencyEquityValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assetRegisterVersionId = await CreateAggregatedAssets(count, schemeId, address, agencyFairValue, agencyEquityValue);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersionId,
                };
                //act
                var assets = await _classUnderTest.Aggregate(aggregatedSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.MovementInAssetValue.Should().Be((agencyFairValue * expectedCount) - (agencyEquityValue * expectedCount));
                trans.Dispose();
            }
        }

        private async Task<int> CreateAggregatedAssets(int count, int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue)
        {
            var entities = new List<IAsset>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(CustomiseGeneratedAssetEntity(schemeId, address, agencyFairValue, agencyEquityValue, _gateway));
            }

            var assets = await _bulkAssetCreator.BulkCreateAsync(new AssetRegisterVersion
            {
                Assets = entities,
                ModifiedDateTime = DateTime.UtcNow,

            }, CancellationToken.None);
            return assets.Select(s => s.AssetRegisterVersionId.Value).FirstOrDefault();
        }

        private IAsset CustomiseGeneratedAssetEntity(int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue, IGateway<IAsset, int> gateway)
        {
            IAsset entity = TestData.Domain.GenerateAsset();
            if (schemeId.HasValue)
                entity.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                entity.Address = address;
            if (agencyFairValue.HasValue)
                entity.AgencyFairValue = agencyFairValue;
            if (agencyEquityValue.HasValue)
                entity.AgencyEquityLoan = agencyEquityValue;
            return entity;
        }

    }
}
