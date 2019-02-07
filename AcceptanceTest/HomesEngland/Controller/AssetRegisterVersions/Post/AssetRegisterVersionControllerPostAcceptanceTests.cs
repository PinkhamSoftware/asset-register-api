using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using FluentSim;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.Notifications;
using HomesEngland.BackgroundProcessing;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.ImportAssets;
using Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using WebApi.Controllers;

namespace AssetRegisterTests.HomesEngland.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostAcceptanceTests
    {
        private const string GovNotifyHost = "http://localhost:8008/";

        private const string GovNotifyApiKey =
            "cafe-cafecafe-cafe-cafe-cafe-cafecafecafe-cafecafe-cafe-cafe-cafe-cafecafecafe";

        private AssetRegisterVersionController _classUnderTest;
        private FluentSimulator _govNotifySimulator;
        private AssetRegisterContext _assetRegisterContext;

        private class NotifyRequest
        {
            public string email_address { get; set; }
            public string template_id { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", GovNotifyHost);
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY", GovNotifyApiKey);

            _govNotifySimulator = new FluentSimulator(GovNotifyHost);
            _govNotifySimulator.Start();
            _govNotifySimulator.Post("/v2/notifications/email").Responds("{}");

            var assetRegister = new AssetRegister();
            var importUseCase = assetRegister.Get<IImportAssetsUseCase>();
            var textSplitter = assetRegister.Get<ITextSplitter>();
            var getAssetRegisterVersionUseCase = assetRegister.Get<IGetAssetRegisterVersionsUseCase>();
            var assetRegisterUploadNotifier = assetRegister.Get<IAssetRegisterUploadProcessedNotifier>();
            var backgroundProcessor = assetRegister.Get<IBackgroundProcessor>();
            _assetRegisterContext = assetRegister.Get<AssetRegisterContext>();
            _classUnderTest = new AssetRegisterVersionController(getAssetRegisterVersionUseCase, importUseCase, textSplitter, assetRegisterUploadNotifier, backgroundProcessor);
        }

        [TearDown]
        public void TearDown()
        {
            _govNotifySimulator.Stop();
            
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCanImport(int expectedCount, string fileValue)
        {
            //arrange
            var formFiles = await GetFormFiles(fileValue);
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AddTokenToHeaderForEmail("stub@stub.com");

                var response = await _classUnderTest.Post(formFiles);
                //asset
                var result = response as StatusCodeResult;
                result.Should().NotBeNull();
                result.StatusCode.Should().Be(200);
                await Task.Delay(2550+ expectedCount * 150);
                _assetRegisterContext.Assets.Select(s => s.Id).Count().Should().Be(expectedCount);
            }
        }

        [TestCase(1, "asset-register-1-rows.csv", "test@test.com")]
        [TestCase(5, "asset-register-5-rows.csv", "cat@meow.com")]
        [TestCase(10, "asset-register-10-rows.csv", "dog@pupper.com")]
        public async Task GivenValidFileAndTokenWithEmail_WhenUploading_SendsEmailNotificationToUploader(
            int expectedCount, string fileValue, string email)
        {
            //arrange
            var formFiles = await GetFormFiles(fileValue);
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                AddTokenToHeaderForEmail(email);

                await _classUnderTest.Post(formFiles);
                await Task.Delay(2550+ expectedCount * 150);

                _govNotifySimulator.ReceivedRequests.Count.Should().Be(1);

                NotifyRequest notifyRequest = _govNotifySimulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

                notifyRequest.email_address.Should().Be(email);
            }
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

        private async Task<List<IFormFile>> GetFormFiles(string fileValue)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "HomesEngland", "Controller", "AssetRegisterVersions", "Post",
                fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            var formFiles = new List<IFormFile>
                {new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)};
            return formFiles;
        }
    }
}
