using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.BackgroundProcessing;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.ImportAssets.Models;
using HomesEngland.UseCase.SaveFile;
using HomesEngland.UseCase.SaveFile.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using Sentry.Extensibility;
using WebApi.Controllers;

namespace WebApiTest.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostTests
    {
        private AssetRegisterVersionController _classUnderTest;
        private Mock<IImportAssetsUseCase> _mockUseCase;
        private Mock<IGetAssetRegisterVersionsUseCase> _mockGetUseCase;
        private ITextSplitter _textSplitter;
        private IBackgroundProcessor _backgroundProcessor;

        [SetUp]
        public void  Setup()
        {
            _mockUseCase = new Mock<IImportAssetsUseCase>();
            _mockGetUseCase = new Mock<IGetAssetRegisterVersionsUseCase>();
            _textSplitter = new TextSplitter();
            _backgroundProcessor = new BackgroundProcessor();
            _classUnderTest = new AssetRegisterVersionController(_mockGetUseCase.Object, _mockUseCase.Object, _textSplitter,_backgroundProcessor );
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10,"asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCallImport(int expectedCount,string fileValue)
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportAssetsResponse
                {
                    AssetsImported = new List<AssetOutputModel>()
                });
            var formFiles = await GetFormFiles(fileValue);
            //act
            var response = await _classUnderTest.Post(formFiles);
            //asset
            response.Should().NotBeNull();
        }

        private async Task<List<IFormFile>> GetFormFiles(string fileValue)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "Controller", "AssetRegisterVersions", "Post", fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            var formFiles = new List<IFormFile>
                {new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)};
            return formFiles;
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5,"asset-register-5-rows.csv")]
        [TestCase(10,"asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenOutputCSV(int expectedCount, string fileValue)
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportAssetsResponse
                {
                    AssetsImported = new List<AssetOutputModel>() {new AssetOutputModel
                    {
                        Id = 1
                    } }
                });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));

            var formFiles = await GetFormFiles(fileValue);
            //act
            var response = await _classUnderTest.Post(formFiles);
            //asset
            var result = response as StatusCodeResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }
    }
}
