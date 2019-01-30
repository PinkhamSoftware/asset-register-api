using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
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

        [SetUp]
        public void  Setup()
        {
            _mockUseCase = new Mock<IImportAssetsUseCase>();
            _mockGetUseCase = new Mock<IGetAssetRegisterVersionsUseCase>();
            _textSplitter = new TextSplitter();
            _classUnderTest = new AssetRegisterVersionController(_mockGetUseCase.Object,_mockUseCase.Object, _textSplitter);
        }

        [TestCase(1, "--file", "asset-register-1-rows.csv", "--delimiter", ";")]
        [TestCase(5, "--file", "asset-register-5-rows.csv", "--delimiter", ";")]
        [TestCase(10, "--file", "asset-register-10-rows.csv", "--delimiter", ";")]
        public async Task GivenValidFile_WhenUploading_ThenCanSaveFile(int expectedCount, string fileFlag, string fileValue, string delimiterFlag, string delimiterValue)
        {
            //arrange
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "Controller", "AssetRegisterVersions","Post", fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            //act
            var response = await _classUnderTest.Post(
                new List<IFormFile>{new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)});
            //asset
            response.Should().NotBeNull();

        }
    }
}
