using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Models;
using Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using WebApi.Controllers;
using WebApi.Extensions;

namespace AssetRegisterTests.HomesEngland.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostTests
    {
        private AssetRegisterVersionController _classUnderTest;

        [SetUp]
        public void  Setup()
        {
            var assetRegister = new AssetRegister();
            var importUseCase = assetRegister.Get<IImportAssetsUseCase>();
            var textSplitter = assetRegister.Get<ITextSplitter>();
            var getAssetRegisterVersionUseCase = assetRegister.Get<IGetAssetRegisterVersionsUseCase>();
            _classUnderTest = new AssetRegisterVersionController(getAssetRegisterVersionUseCase, importUseCase,textSplitter);
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCanImport(int expectedCount,string fileValue)
        {
            //arrange
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory,"HomesEngland", "Controller", "AssetRegisterVersions","Post", fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.Post(
                    new List<IFormFile> {new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)});
                //asset
                var result = response as ObjectResult;
                result.Should().NotBeNull();
                result.Value.Should().BeOfType<ResponseData<ImportAssetsResponse>>();
                var data = result.Value as ResponseData<ImportAssetsResponse>;
                data.Data.AssetsImported.Count.Should().Be(expectedCount);
            }
        }
    }
}
