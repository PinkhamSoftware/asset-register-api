using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TestHelper;
using WebApi.Extensions.Requests;

namespace WebApiTest.Controller.Asset.Get
{
    [TestFixture]
    public class AssetControllerTests
    {
        private readonly AssetController _classUnderTest;
        private readonly Mock<IGetAssetUseCase> _mockUseCase;

        public AssetControllerTests()
        {
            _mockUseCase = new Mock<IGetAssetUseCase>();
            _classUnderTest = new AssetController(_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidRequest_ThenReturnsGetAssetResponse()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<GetAssetRequest>())).ReturnsAsync(new GetAssetResponse());
            var request = new GetAssetApiRequest();
            //act
            var response = await _classUnderTest.Get(request);
            //assert
            response.Should().NotBeNull();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GivenValidRequestWithAcceptTextCsvHeader_ThenReturnsListOfAssetOutputModel(int id)
        {
            //arrange
            AssetOutputModel assetOutputModel = new AssetOutputModel(TestData.Domain.GenerateAsset()) {Id = id};
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<GetAssetRequest>())).ReturnsAsync(new GetAssetResponse
            {
                Asset = assetOutputModel
            });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));
            var request = new GetAssetApiRequest
            {
                Id = id
            };
            //act
            IActionResult response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetOutputModel>>();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(null)]
        public void GivenInValidRequest_ThenThrowsBadRequestException(int id)
        {
            //arrange
            var request = new GetAssetApiRequest
            {
                Id = id
            };

            request.IsValid().Should().BeFalse();
        }
    }
}
