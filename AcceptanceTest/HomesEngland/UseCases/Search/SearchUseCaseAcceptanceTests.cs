using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GetAssetDevelopers;
using HomesEngland.UseCase.GetAssetRegions;
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
        private readonly IGetAssetRegionsUseCase _getAssetRegionsUseCase;
        private readonly IGetAssetDevelopersUseCase _getAssetDevelopersUseCase;

        public SearchUseCaseAcceptanceTests()
        {
            var assetRegister = new AssetRegister();

            _createAssetRegisterVersionUseCase = assetRegister.Get<ICreateAssetRegisterVersionUseCase>();
            _classUnderTest = assetRegister.Get<ISearchAssetUseCase>();
            _getAssetRegionsUseCase = assetRegister.Get<IGetAssetRegionsUseCase>();
            _getAssetDevelopersUseCase = assetRegister.Get<IGetAssetDevelopersUseCase>();

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
                var foundAsset = await SearchForAssetAsync(schemeId, searchAddress, responses.GetAssetRegisterVersionId(), null, null);
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        private async Task<SearchAssetResponse> SearchForAssetAsync(int? schemeId, string address, int assetRegisterVersionId, string region, string developer = null)
        {
            var searchForAsset = new SearchAssetRequest
            {
                SchemeId = schemeId,
                Address = address,
                AssetRegisterVersionId = assetRegisterVersionId,
                Region = region,
                Developer = developer
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
                    CreateAsset()
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
                    var createAssetRequest = CreateAsset(schemeId + i, address);
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

        [TestCase("Region 1", "Regi")]
        [TestCase("Region 2", "Regio")]
        [TestCase("Region 3", "Region 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaRegion_ThenWeCanFindTheSameAsset(string region, string searchRegion)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, null,region),
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                //act
                var foundAsset = await SearchForAssetAsync(null,null, responses.GetAssetRegisterVersionId(), searchRegion, null);
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        [TestCase("Region 4")]
        [TestCase("Region 5")]
        [TestCase("Region 6")]
        public async Task GivenAnAssetHasBeenCreated_WhenGetGetRegionFilters_AndWeSearchViaRegion_ThenWeCanFindTheSameAsset(string region)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, null,region),
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var regions = await _getAssetRegionsUseCase.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                //act
                var foundAsset = await SearchForAssetAsync(null, null, responses.GetAssetRegisterVersionId(), regions.Regions[0].Name);
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

            trans.Dispose();
        }
    }

        [TestCase(1114, "Developer 1", "Random 1", 5, 1, 2, 2, 2, 3)]
        [TestCase(2224, "Developer 2", "Random 2", 5, 1, 2, 2, 2, 3)]
        [TestCase(3334, "Developer 3", "Random 3", 5, 1, 2, 2, 2, 3)]
        public async Task GivenMultiplePages_WhenWeSearchByDeveloperThatMatches_ThenWeReturnOnlyTheSelectedPage(int schemeId, string developer, string randomString, int createdCount, int expectedCount, int page, int pageSize, int expectedPageCount, int expectedTotalCount)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < createdCount; i++)
                {
                    var dev = i % 2 == 0 ? developer : randomString;
                    var createAssetRequest = CreateAsset(schemeId + i, null, null, dev);
                    list.Add(createAssetRequest);
                }

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var assetSearch = new SearchAssetRequest
                {
                    Page = page,
                    PageSize = pageSize,
                    AssetRegisterVersionId = responses.GetAssetRegisterVersionId(),
                    Developer = developer
                    
                };

                var response = await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Assets.Count.Should().Be(expectedCount);
                response.Pages.Should().Be(expectedPageCount);
                response.TotalCount.Should().Be(expectedTotalCount);
                trans.Dispose();
            }
        }

        [TestCase("Developer 1", "Dev")]
        [TestCase("Developer 2", "Devel")]
        [TestCase("Developer 3", "veloper 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaDeveloper_ThenWeCanFindTheSameAsset(string developer, string searchDeveloper)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, null, null, developer),
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                //act
                var foundAsset = await SearchForAssetAsync(null,null, responses.GetAssetRegisterVersionId(), null, developer);
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        [TestCase("Developer 4")]
        [TestCase("Developer 5")]
        [TestCase("Developer 6")]
        public async Task GivenAnAssetHasBeenCreated_WhenGetGetDeveloperFilters_AndWeSearchViaDeveloper_ThenWeCanFindTheSameAsset(string developer)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>
                {
                    CreateAsset(null, null,null, developer),
                };

                var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var developers = await _getAssetDevelopersUseCase.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                //act
                var foundAsset = await SearchForAssetAsync(null, null, responses.GetAssetRegisterVersionId(), null, developers.Developers[0].Name);
                //assert
                ExpectFoundAssetIsEqual(foundAsset, responses[0]);

                trans.Dispose();
            }
        }

        private CreateAssetRequest CreateAsset(int? schemeId = null, string address = null, string region = null, string developer = null)
        {
            CreateAssetRequest createAssetRequest = TestData.UseCase.GenerateCreateAssetRequest();
            if (schemeId.HasValue)
                createAssetRequest.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                createAssetRequest.Address = address;
            if (!string.IsNullOrEmpty(region))
            {
                createAssetRequest.ImsOldRegion = region;
            }

            if (!string.IsNullOrEmpty(developer))
            {
                createAssetRequest.DevelopingRslName = developer;
            }

            return createAssetRequest;
        }
    }
}
