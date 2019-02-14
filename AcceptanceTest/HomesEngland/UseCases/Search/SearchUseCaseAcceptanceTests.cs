using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Main;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.UseCases.Search
{
    [TestFixture]
    public class SearchUseCaseAcceptanceTests
    {
        private readonly ICreateAssetRegisterVersionUseCase _createAssetRegisterVersionUseCase;
        private readonly ISearchAssetUseCase _classUnderTest;

        public SearchUseCaseAcceptanceTests()
        {
            var assetRegister = new AssetRegister();

            _createAssetRegisterVersionUseCase = assetRegister.Get<ICreateAssetRegisterVersionUseCase>();
            _classUnderTest = assetRegister.Get<ISearchAssetUseCase>();

            var assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
        }

        [TestCase(1111, null, null)]
        [TestCase(2222, null, null)]
        [TestCase(3333, null, null)]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "add")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "Address 1")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "somewh")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "where")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "PO")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "Tow")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "C03")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearch_ThenWeCanFindTheSameAsset(int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(schemeId, address),                    
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);
                
                //act
                var foundAsset = await SearchForAssetAsync(schemeId, searchAddress, responses.GetAssetRegisterVersionId());
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        private async Task<SearchAssetResponse> SearchForAssetAsync(int? schemeId, string address, int assetRegisterVersionId)
        {
            var searchForAsset = new SearchAssetRequest
            {
                SchemeId = schemeId,
                Address = address,
                AssetRegisterVersionId = assetRegisterVersionId
            };

            var useCaseResponse = await _classUnderTest.ExecuteAsync(searchForAsset, CancellationToken.None)
                .ConfigureAwait(false);
            return useCaseResponse;
        }

        private void ExpectFoundAssetIsEqual(SearchAssetResponse foundAsset, CreateAssetResponse createdAsset)
        {
            foundAsset.Should().NotBeNull();
            foundAsset.Assets.Should().NotBeNullOrEmpty();
            foundAsset.Assets.ElementAt(0).AssetOutputModelIsEqual(createdAsset.Asset);
        }

        [TestCase(7777, 7778, 7779, "address")]
        [TestCase(2222, 2224, 2225, "address")]
        [TestCase(3333, 3334, 3335, "address")]
        public async Task GivenThatMultipleAssetsHaveBeenCreated_WhenWeSearchViaSchemeIdThatHasBeenSet_ThenWeCanFindTheSameAssetAnd(int schemeId, int schemeId2, int schemeId3, string address)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                List<CreateAssetRequest> list = new List<CreateAssetRequest>
                {
                    CreateAsset(schemeId, address),
                    CreateAsset(schemeId2, address),
                    CreateAsset(schemeId3, address),
                };

                IList<CreateAssetResponse> responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var assetSearch = new SearchAssetRequest
                {
                    SchemeId = schemeId2,
                    AssetRegisterVersionId = responses.GetAssetRegisterVersionId()
                };
                //act
                var useCaseResponse = await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None)
                    .ConfigureAwait(false);
                //assert
                useCaseResponse.Assets.Count.Should().Be(1);
                useCaseResponse.Assets.ElementAtOrDefault(0).AssetOutputModelIsEqual(responses[1].Asset);
                trans.Dispose();
            }
        }

        [TestCase(4444)]
        [TestCase(5555)]
        [TestCase(6666)]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaSchemeIdThatHasntBeenSet_ThenWeReturnNullOrEmptyList(int schemeId)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, null)
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var assetSearch = new SearchAssetRequest
                {
                    SchemeId = schemeId,
                    AssetRegisterVersionId = responses.GetAssetRegisterVersionId()
                };
                //act 
                //assert
                var response = await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None)
                    .ConfigureAwait(false);
                response.Should().NotBeNull();
                response.Assets.Should().BeNullOrEmpty();
                trans.Dispose();
            }
        }

        [TestCase(1111, "Address 1")]
        [TestCase(2222, "Address 2")]
        [TestCase(3333, "Address 3")]
        public async Task GivenMultiplePages_WhenWeSearchByAddressThatMatches_ThenWeReturnOnlyTheSelectedPage(int schemeId, string address)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < 15; i++)
                {
                    var createAssetRequest = CreateAsset(schemeId + i, address, null);
                    list.Add(createAssetRequest);
                }

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var assetSearch = new SearchAssetRequest
                {
                    Address = address,
                    Page = 2,
                    PageSize = 10,
                    AssetRegisterVersionId = responses.GetAssetRegisterVersionId()
                };

                var response = await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Assets.Count.Should().Be(5);
                response.Pages.Should().Be(2);
                response.TotalCount.Should().Be(15);
                trans.Dispose();
            }
        }

        [TestCase(1114, "Region 1")]
        [TestCase(2224, "Region 2")]
        [TestCase(3334, "Region 3")]
        public async Task GivenMultiplePages_WhenWeSearchByRegionThatMatches_ThenWeReturnOnlyTheSelectedPage(int schemeId, string region)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < 15; i++)
                {
                    var createAssetRequest = CreateAsset(schemeId + i, null, region);
                    list.Add(createAssetRequest);
                }

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var assetSearch = new SearchAssetRequest
                {
                    Region = region,
                    Page = 2,
                    PageSize = 10,
                    AssetRegisterVersionId = responses.GetAssetRegisterVersionId()
                };

                var response = await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Assets.Count.Should().Be(5);
                response.Pages.Should().Be(2);
                response.TotalCount.Should().Be(15);
                trans.Dispose();
            }
        }

        [TestCase( "Region 1", "Regi")]
        [TestCase( "Region 2", "Regio")]
        [TestCase( "Region 3", "Region 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaRegion_ThenWeCanFindTheSameAsset(string region, string searchRegion)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, region),
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                //act
                var foundAsset = await SearchForAssetAsync(null, searchRegion, responses.GetAssetRegisterVersionId());
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        private CreateAssetRequest CreateAsset(int? schemeId, string address, string region = null)
        {
            CreateAssetRequest createAssetRequest = TestData.UseCase.GenerateCreateAssetRequest();
            if (schemeId.HasValue)
                createAssetRequest.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                createAssetRequest.Address = address;
            if (!string.IsNullOrEmpty(region))
            {
                createAssetRequest.LocationLaRegionName = region;
                createAssetRequest.ImsOldRegion = region;
            }

            return createAssetRequest;
        }
    }
}
