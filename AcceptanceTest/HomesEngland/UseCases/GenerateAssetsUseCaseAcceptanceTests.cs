using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GenerateAssets.Models;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Infrastructure.Api.Exceptions;
using Main;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.UseCases
{
    [TestFixture]
    public class GenerateAssetsUseCaseTest
    {
        private readonly IGenerateAssetsUseCase _classUnderTest;
        private readonly ISearchAssetUseCase _searchAssetUseCase;
        public GenerateAssetsUseCaseTest()
        {
            var assetRegister = new AssetRegister();
            
            _classUnderTest = assetRegister.Get<IGenerateAssetsUseCase>();
            _searchAssetUseCase = assetRegister.Get<ISearchAssetUseCase>();

            var context = assetRegister.Get<AssetRegisterContext>();
            context.Database.Migrate();
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GivenWeGenerateSomeAssets_ThenWeKnowHowManyAssetsWereCreated(int recordCount)
        {
            //arrange 
            var request = new GenerateAssetsRequest
            {
                Records = recordCount
            };
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None)
                    .ConfigureAwait(false);
                //assert
                response.Should().NotBeNull();
                response.RecordsGenerated.Count.Should().Be(recordCount);
                trans.Dispose();
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GivenWeGenerateSomeAssets_WhenWeSearchForAssets_ThenWeCanFindThoseRecords(int recordCount)
        {
            //arrange 
            var request = new GenerateAssetsRequest
            {
                Records = recordCount
            };
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);
                //assert
                for (int i = 0; i < response.RecordsGenerated.Count; i++)
                {
                    var generatedAsset = response.RecordsGenerated.ElementAtOrDefault(i);

                    var record = await FindAsset(generatedAsset, i);

                    record.Should().NotBeNull();
                    record.AssetOutputModelIsEqual(generatedAsset);
                }
                trans.Dispose();
            }
        }

        private async Task<AssetOutputModel> FindAsset(AssetOutputModel generatedAsset, int i)
        {
            var record = await _searchAssetUseCase.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = generatedAsset?.SchemeId
            }, CancellationToken.None).ConfigureAwait(false);
            return record.Assets.ElementAtOrDefault(0);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(null)]
        public void GivenInValidRequest_ThenUseCaseThrowsBadRequestException(int? recordCount)
        {
            //arrange 
            var request = new GenerateAssetsRequest
            {
                Records = recordCount
            };
            //act
            //assert
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false));
                trans.Dispose();
            }
        }
    }
}
