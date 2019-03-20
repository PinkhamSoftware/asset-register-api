using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GetAsset.Models;
using Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.UseCases.CalculateAssetAggregates
{
    [TestFixture]
    public class CalculateAssetAggregatesUseCaseAcceptanceTests
    {
        private readonly ICalculateAssetAggregatesUseCase _classUnderTest;
        private readonly ICreateAssetRegisterVersionUseCase _createAssetRegisterVersionUseCase;

        public CalculateAssetAggregatesUseCaseAcceptanceTests()
        {
            IConfigurationRoot configuration = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory());

            var assetRegister = new AssetRegister(configuration);

            _createAssetRegisterVersionUseCase = assetRegister.Get<ICreateAssetRegisterVersionUseCase>();
            _classUnderTest = assetRegister.Get<ICalculateAssetAggregatesUseCase>();

            var assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
        }

        [TestCase(3, 1, 1111, null, null)]
        [TestCase(3, 1, 2222, null, null)]
        [TestCase(3, 1, 3333, null, null)]
        [TestCase(3, 3, null, "Address 10, Somewhere road, City, Region, PO57 C03", "Address 10")]
        [TestCase(2, 2, null, "Address 10, Somewhere road, City, Region, PO57 C03", "somewh")]
        [TestCase(1, 1, null, "Address 10, Somewhere road, City, Region, PO57 C03", "where")]
        [TestCase(3, 3, null, "Address 10, Somewhere road, City, Region, PO57 C03", "PO57")]
        [TestCase(3, 3, null, "Address 10, Somewhere road, City, Region, PO57 C03", "City")]
        [TestCase(3, 3, null, "Address 10, Somewhere road, City, Region, PO57 C03", "C03")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateBasedOnSearchCriteria_ThenWeCanGetTheUniqueRecordsCount(int createdCount, int expectedCount, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = await GenerateAssetsForAggregation(createdCount, schemeId, address, null, null);
                //act
                var searchAggregate = await CalculateAggregatesForSearchCriteria(schemeId, searchAddress, assets.GetAssetRegisterVersionId());
                //assert
                searchAggregate.AssetAggregates.UniqueRecords.Should().Be(expectedCount);
                trans.Dispose();
            }
        }

        [TestCase(3, 3, 1000.01, 1111, null, null)]
        [TestCase(3, 3, 1000.01, 2222, null, null)]
        [TestCase(3, 3, 1000.01, 3333, null, null)]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "Address 10")]
        [TestCase(2, 2, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "somewh")]
        [TestCase(1, 1, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "where")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "PO57")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "City")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "C03")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateBasedOnSearchCriteria_ThenWeCanGetTheMoneyPaidOut(int createdCount, int expectedCount, decimal? agencyEquityValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = await GenerateAssetsForAggregation(createdCount, schemeId, address, null, agencyEquityValue);
                //act
                var searchAggregate = await CalculateAggregatesForSearchCriteria(schemeId, searchAddress, assets.GetAssetRegisterVersionId());
                //assert
                searchAggregate.AssetAggregates.MoneyPaidOut.Should().Be(expectedCount * agencyEquityValue);
                trans.Dispose();
            }
        }

        [TestCase(3, 3, 1000.01, 1111, null, null)]
        [TestCase(3, 3, 1000.01, 2222, null, null)]
        [TestCase(3, 3, 1000.01, 3333, null, null)]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "Address 10")]
        [TestCase(2, 2, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "somewh")]
        [TestCase(1, 1, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "where")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "PO57")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "City")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "C03")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateBasedOnSearchCriteria_ThenWeCanGetTheAssetValue(int createdCount, int expectedCount, decimal? agencyFairValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = await GenerateAssetsForAggregation(createdCount, schemeId, address, agencyFairValue, null);
                //act
                var searchAggregate = await CalculateAggregatesForSearchCriteria(schemeId, searchAddress, assets.GetAssetRegisterVersionId());
                //assert
                searchAggregate.AssetAggregates.AssetValue.Should().Be(expectedCount * agencyFairValue);
                trans.Dispose();
            }
        }

        [TestCase(3, 3, 1000.01, 1111, null, null)]
        [TestCase(3, 3, 1000.01, 2222, null, null)]
        [TestCase(3, 3, 1000.01, 3333, null, null)]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "Address 10")]
        [TestCase(2, 2, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "somewh")]
        [TestCase(1, 1, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "where")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "PO57")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "City")]
        [TestCase(3, 3, 1000.01, null, "Address 10, Somewhere road, City, Region, PO57 C03", "C03")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateBasedOnSearchCriteria_ThenWeCanGetMoneyPaidOut(int createdCount, int expectedCount, decimal? agencyEquityValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = await GenerateAssetsForAggregation(createdCount, schemeId, address, null, agencyEquityValue);
                //act
                var searchAggregate = await CalculateAggregatesForSearchCriteria(schemeId, searchAddress, assets.GetAssetRegisterVersionId());
                //assert
                searchAggregate.AssetAggregates.MoneyPaidOut.Should().Be(expectedCount * agencyEquityValue);
                trans.Dispose();
            }
        }

        [TestCase(3, 3, 1000.01, 301.301, 1111, null, null)]
        [TestCase(3, 3, 1000.01, 301.301, 2222, null, null)]
        [TestCase(3, 3, 1000.01, 301.301, 3333, null, null)]
        [TestCase(3, 3, 1000.01, 301.301, null, "Address 10, Somewhere road, City, Region, PO57 C03", "Address 10")]
        [TestCase(2, 2, 1000.01, 301.301, null, "Address 10, Somewhere road, City, Region, PO57 C03", "somewh")]
        [TestCase(1, 1, 1000.01, 301.301, null, "Address 10, Somewhere road, City, Region, PO57 C03", "where")]
        [TestCase(3, 3, 1000.01, 301.301, null, "Address 10, Somewhere road, City, Region, PO57 C03", "PO57")]
        [TestCase(3, 3, 1000.01, 301.301, null, "Address 10, Somewhere road, City, Region, PO57 C03", "City")]
        [TestCase(3, 3, 1000.01, 301.301,  null, "Address 10, Somewhere road, City, Region, PO57 C03", "C03")]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeAggregateBasedOnSearchCriteria_ThenWeCanGetMovementInFairValue(int createdCount, int expectedCount, decimal? agencyFairValue, decimal? agencyEquityValue, int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assetEntities = await GenerateAssetsForAggregation(createdCount, schemeId, address, agencyFairValue, agencyEquityValue);
                //act
                var searchAggregate = await CalculateAggregatesForSearchCriteria(schemeId, searchAddress, assetEntities.GetAssetRegisterVersionId());
                //assert
                searchAggregate.AssetAggregates.MovementInAssetValue.Should().Be((agencyFairValue * expectedCount) - (expectedCount * agencyEquityValue));
                trans.Dispose();
            }
        }

        private async Task<IList<AssetOutputModel>> GenerateAssetsForAggregation(int createdCount, int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue)
        {
            var list = new List<CreateAssetRequest>();
            for (int i = 0; i < createdCount; i++)
            {
                list.Add(CreateAsset(schemeId, address, agencyFairValue, agencyEquityValue));
            }

            var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);
            return responses.Select(s=> s.Asset).ToList();
        }

        private async Task<CalculateAssetAggregateResponse> CalculateAggregatesForSearchCriteria(int? schemeId, string address, int assetRegisterVersionId)
        {
            var searchForAsset = new CalculateAssetAggregateRequest
            {
                SchemeId = schemeId,
                Address = address,
                AssetRegisterVersionId = assetRegisterVersionId
            };

            var useCaseResponse = await _classUnderTest.ExecuteAsync(searchForAsset, CancellationToken.None).ConfigureAwait(false);
            return useCaseResponse;
        }

        private CreateAssetRequest CreateAsset(int? schemeId, string address, decimal? agencyFairValue, decimal? agencyEquityValue)
        {
            var createAssetRequest = TestData.UseCase.GenerateCreateAssetRequest();
            if (schemeId.HasValue)
                createAssetRequest.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                createAssetRequest.Address = address;
            if (agencyFairValue.HasValue)
                createAssetRequest.AgencyFairValue = agencyFairValue;
            if (agencyEquityValue.HasValue)
                createAssetRequest.AgencyEquityLoan = agencyEquityValue;
            return createAssetRequest;
        }
    }
}
