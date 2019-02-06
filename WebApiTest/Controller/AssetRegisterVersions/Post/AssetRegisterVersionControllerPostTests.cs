using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Notifications;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.ImportAssets.Models;
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
        private Mock<IAssetRegisterUploadProcessedNotifier> _assetRegisterUploadProcessedNotifier;

        [SetUp]
        public void Setup()
        {
            _mockUseCase = new Mock<IImportAssetsUseCase>();
            _mockGetUseCase = new Mock<IGetAssetRegisterVersionsUseCase>();
            _textSplitter = new TextSplitter();
            _assetRegisterUploadProcessedNotifier = new Mock<IAssetRegisterUploadProcessedNotifier>();
            _classUnderTest = new AssetRegisterVersionController(_mockGetUseCase.Object, _mockUseCase.Object,
                _textSplitter, _assetRegisterUploadProcessedNotifier.Object);
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCallImport(int expectedCount, string fileValue)
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportAssetsResponse
                {
                    AssetsImported = new List<AssetOutputModel>()
                });
            AddTokenToHeaderForEmail("stub@stub.com");
            var formFiles = await GetFormFiles(fileValue);
            //act
            var response = await _classUnderTest.Post(formFiles);
            //asset
            response.Should().NotBeNull();
        }

        [TestCase("asset-register-1-rows.csv", "test@test.com")]
        [TestCase("asset-register-5-rows.csv", "dog@woof.com")]
        [TestCase("asset-register-10-rows.csv", "cat@meow.com")]
        public async Task GivenValidFile_WhenUploading_NotifyThePersonWhoUploadedIt(string fileValue, string email)
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportAssetsResponse
                {
                    AssetsImported = new List<AssetOutputModel>()
                });
            AddTokenToHeaderForEmail(email);

            var formFiles = await GetFormFiles(fileValue);
            //act
            await _classUnderTest.Post(formFiles);
            //asset
            _assetRegisterUploadProcessedNotifier.Verify(o =>
                o.SendUploadProcessedNotification(It.Is<IUploadProcessedNotification>(n => n.Email.Equals(email)),
                    It.IsAny<CancellationToken>()));
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenOutputCSV(int expectedCount, string fileValue)
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportAssetsResponse
                {
                    AssetsImported = new List<AssetOutputModel>
                    {
                        new AssetOutputModel
                        {
                            Id = 1
                        }
                    }
                });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("Authorization",
                    $"Bearer {CreateAuthTokenForEmail("stub@stub.com")}"));


            var formFiles = await GetFormFiles(fileValue);
            //act
            var response = await _classUnderTest.Post(formFiles);
            //asset
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetOutputModel>>();
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

        private void AddTokenToHeaderForEmail(string email)
        {
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("Authorization", $"Bearer {CreateAuthTokenForEmail(email)}"));
        }

        private string CreateAuthTokenForEmail(string email)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            Claim emailClaim = new Claim("email", email);

            List<Claim> claims = new List<Claim> {emailClaim};

            string tokenString = tokenHandler.WriteToken(tokenHandler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(claims)
            ));

            return tokenString;
        }
    }
}
