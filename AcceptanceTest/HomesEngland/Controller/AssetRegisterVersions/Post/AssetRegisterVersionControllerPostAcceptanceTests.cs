using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.BackgroundProcessing;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.ImportAssets;
using Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApi.Controllers;

namespace AssetRegisterTests.HomesEngland.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostAcceptanceTests
    {
        private AssetRegisterVersionController _classUnderTest;
        private AssetRegisterContext _assetRegisterContext;

        [SetUp]
        public void Setup()
        {
            var assetRegister = new AssetRegister();
            var importUseCase = assetRegister.Get<IImportAssetsUseCase>();
            var textSplitter = assetRegister.Get<ITextSplitter>();
            var getAssetRegisterVersionUseCase = assetRegister.Get<IGetAssetRegisterVersionsUseCase>();
            var backgroundProcessor = assetRegister.Get<IBackgroundProcessor>();
            _assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            _classUnderTest = new AssetRegisterVersionController(getAssetRegisterVersionUseCase, importUseCase,textSplitter, backgroundProcessor);
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCanImport(int expectedCount,string fileValue)
        {
            //arrange
            var formFiles = await GetFormFiles(fileValue);
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.Post(formFiles);
                //asset
                
                
                var result = response as StatusCodeResult;
                result.Should().NotBeNull();
                result.StatusCode.Should().Be(200);
                await Task.Delay(50000);
                _assetRegisterContext.Assets.Select(s => s.Id).Count().Should().Be(expectedCount);
            }
        }

        private async Task<List<IFormFile>> GetFormFiles(string fileValue)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory,"HomesEngland", "Controller", "AssetRegisterVersions", "Post", fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            var formFiles = new List<IFormFile>
                {new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)};
            return formFiles;
        }
    }
}
