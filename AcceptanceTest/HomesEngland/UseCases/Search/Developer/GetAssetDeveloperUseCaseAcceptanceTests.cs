using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GetAssetDevelopers;
using Main;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.UseCases.Search.Developer
{
    [TestFixture]
    public class GetAssetDeveloperUseCaseAcceptanceTests
    {
        private readonly ICreateAssetRegisterVersionUseCase _createAssetRegisterVersionUseCase;
        private readonly IGetAssetDevelopersUseCase _classUnderTest;

        public GetAssetDeveloperUseCaseAcceptanceTests()
        {
            var assetRegister = new AssetRegister();

            _createAssetRegisterVersionUseCase = assetRegister.Get<ICreateAssetRegisterVersionUseCase>();
            _classUnderTest = assetRegister.Get<IGetAssetDevelopersUseCase>();

            var assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
        }

        [TestCase("Develop ", 3)]
        [TestCase("Dev ", 3)]
        [TestCase("Dever ", 2)]
        public async Task GivenWeHaveXUniqueDevelopers_WhenWeGetAllDevelopers_ThenReturnsXUniqueDevelopers(string developer, int count)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < count; i++)
                {
                    var createAssetRequest = CreateAsset(developer + i);
                    list.Add(createAssetRequest);
                }

                await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var response = await _classUnderTest.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Developers.Should().NotBeNullOrEmpty();
                response.Developers.Count.Should().Be(count);
            }
        }

        [TestCase("Develop ", 3)]
        [TestCase("Dev ", 3)]
        [TestCase("Dever ", 2)]
        public async Task GivenWeHaveXDevelopers_WhenWeGetAllDevelopers_ThenReturnsUniqueDevelopers(string developer, int count)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = new List<CreateAssetRequest>();
                for (var i = 0; i < count; i++)
                {
                    var createAssetRequest = CreateAsset(developer);
                    list.Add(createAssetRequest);
                }

                await _createAssetRegisterVersionUseCase.ExecuteAsync(list, CancellationToken.None).ConfigureAwait(false);

                var response = await _classUnderTest.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Developers.Should().NotBeNullOrEmpty();
                response.Developers.Count.Should().Be(1);
            }
        }

        private CreateAssetRequest CreateAsset(string developer)
        {
            CreateAssetRequest createAssetRequest = TestData.UseCase.GenerateCreateAssetRequest();
            if (!string.IsNullOrEmpty(developer))
            {
                createAssetRequest.DevelopingRslName = developer;
            }

            return createAssetRequest;
        }
    }
}
