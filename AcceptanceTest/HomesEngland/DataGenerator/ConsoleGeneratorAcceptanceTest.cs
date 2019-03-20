using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.DataGenerator
{
    [TestFixture]
    public class ConsoleGeneratorAcceptanceTests
    {
        private readonly IConsoleGenerator _classUnderTest;
        private readonly ISearchAssetUseCase _searchAssetUseCase;

        public ConsoleGeneratorAcceptanceTests()
        {
            IConfigurationRoot configuration = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory());

            var assetRegister = new AssetRegister(configuration);

            _classUnderTest = assetRegister.Get<IConsoleGenerator>();
            _searchAssetUseCase = assetRegister.Get<ISearchAssetUseCase>();

            var context = assetRegister.Get<AssetRegisterContext>();
            context.Database.Migrate();
        }

        [TestCase("--records", "1")]
        [TestCase("--records", "2")]
        [TestCase("--records", "3")]
        public async Task GivenWeNeedToGenerateAssets_WhenWeDoSoThroughAConsoleInterface_ThenWeCanSearchForAssets(string arg1, string arg2)
        {
            //arrange
            var args = new string[] {arg1, arg2};
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
                //assert
                for (int i = 0; i < response.Count; i++)
                {
                    var generatedAsset = response.ElementAtOrDefault(i);

                    var record = await FindAsset(generatedAsset, generatedAsset.AssetRegisterVersionId.Value);
                    
                    record.Should().NotBeNull();
                    record.AssetOutputModelIsEqual(generatedAsset);
                }
                trans.Dispose();
            }
        }

        private async Task<AssetOutputModel> FindAsset(AssetOutputModel generatedAsset, int assetRegisterVersionId)
        {
            var record = await _searchAssetUseCase.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = generatedAsset?.SchemeId,
                AssetRegisterVersionId = assetRegisterVersionId
                
            }, CancellationToken.None).ConfigureAwait(false);
            return record.Assets.ElementAtOrDefault(0);
        }
    }
}
