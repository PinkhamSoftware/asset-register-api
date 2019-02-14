using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Main;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.UseCases.Search.Region
{
    [TestFixture]
    public class GetAssetRegionsUseCaseAcceptanceTests
    {
        private readonly ICreateAssetRegisterVersionUseCase _createAssetRegisterVersionUseCase;
        private readonly IGetAssetRegionsUseCase _classUnderTest;

        public GetAssetRegionsUseCaseAcceptanceTests()
        {
            var assetRegister = new AssetRegister();

            _createAssetRegisterVersionUseCase = assetRegister.Get<ICreateAssetRegisterVersionUseCase>();
            _classUnderTest = assetRegister.Get<IGetAssetRegionsUseCase>();

            var assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("Region ", 3)]
        [TestCase("Reg ", 3)]
        [TestCase("Reggie ",2)]
        public async Task GivenWeHaveXUniqueRegions_WhenWeGetAllRegions_ThenReturnsXUniqueRegions(string region, int count)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < count; i++)
                {
                    var createAssetRequest = CreateAsset(region + i);
                    list.Add(createAssetRequest);
                }

                await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var response = await _classUnderTest.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Regions.Should().NotBeNullOrEmpty();
                response.Regions.Count.Should().Be(count);
            }
        }

        [TestCase("Region", 3)]
        [TestCase("Reg", 3)]
        [TestCase("Reggie", 2)]
        public async Task GivenWeHaveXRegions_WhenWeGetAllRegions_ThenReturnsUniqueRegions(string region, int count)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < count; i++)
                {
                    var createAssetRequest = CreateAsset(region);
                    list.Add(createAssetRequest);
                }

                await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var response = await _classUnderTest.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Regions.Should().NotBeNullOrEmpty();
                response.Regions.Count.Should().Be(1);
            }
        }

        private void ExpectFoundAssetIsEqual(SearchAssetResponse foundAsset, CreateAssetResponse createdAsset)
        {
            foundAsset.Should().NotBeNull();
            foundAsset.Assets.Should().NotBeNullOrEmpty();
            foundAsset.Assets.ElementAt(0).AssetOutputModelIsEqual(createdAsset.Asset);
        }

        private CreateAssetRequest CreateAsset( string region)
        {
            CreateAssetRequest createAssetRequest = TestData.UseCase.GenerateCreateAssetRequest();
            if (!string.IsNullOrEmpty(region))
            {
                createAssetRequest.LocationLaRegionName = region;
                createAssetRequest.ImsOldRegion = region;
            }
                
            return createAssetRequest;
        }
    }
}
