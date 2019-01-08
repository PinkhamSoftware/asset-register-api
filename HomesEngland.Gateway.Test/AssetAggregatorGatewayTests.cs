using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Main;
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

        public AssetAggregatorGatewayTests()
        {
            var assetRegister = new AssetRegister();
            var assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
            _gateway = assetRegister.Get<IGateway<IAsset, int>>();
            _classUnderTest = assetRegister.Get<IAssetAggregator>();
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
                await CreateAggregatedAssets(count, schemeId, address, null, null);
                
                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
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
                await CreateAggregatedAssets(count, schemeId, address, agencyFairValue,null);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
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
                await CreateAggregatedAssets(count, schemeId, address, null, agencyEquityValue);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
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
                await CreateAggregatedAssets(count, schemeId, address, agencyFairValue, agencyEquityValue);

                var aggregatedSearch = new AssetSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
                };
                //act
                var assets = await _classUnderTest.Aggregate(aggregatedSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.MovementInAssetValue.Should().Be((agencyFairValue * expectedCount) - (agencyEquityValue * expectedCount));
                trans.Dispose();
            }
        }

        private async Task CreateAggregatedAssets(int count, int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue)
        {
            for (int i = 0; i < count; i++)
            {
                await CreateAsset(schemeId, address, agencyFairValue, agencyEquityValue, _gateway);
            }
        }

        private async Task<IAsset> CreateAsset(int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue, IGateway<IAsset, int> gateway)
        {
            var entity = TestData.Domain.GenerateAsset();
            if (schemeId.HasValue)
                entity.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                entity.Address = address;
            if (agencyFairValue.HasValue)
                entity.AgencyFairValue = agencyFairValue;
            if (agencyEquityValue.HasValue)
                entity.AgencyEquityLoan = agencyEquityValue;
            IAsset createdAsset = await gateway.CreateAsync(entity).ConfigureAwait(false);
            return createdAsset;
        }

    }
}
