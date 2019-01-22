using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using TestHelper;
using WebApi.Controllers;
using WebApi.Extensions.Requests;

namespace WebApiTest.Controller.AssetRegisterVersions
{
    [TestFixture]
    public class AssetRegisterVersionControllerTests
    {
        private readonly AssetRegisterVersionController _classUnderTest;
        private readonly Mock<IGetAssetRegisterVersionsUseCase> _mockUseCase;

        public AssetRegisterVersionControllerTests()
        {
            _mockUseCase = new Mock<IGetAssetRegisterVersionsUseCase>();
            _classUnderTest = new AssetRegisterVersionController(_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidRequest_ThenReturnsGetAssetResponse()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<GetAssetRegisterVersionsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetRegisterVersionsResponse());
            var request = new SearchAssetApiRequest();
            //act
            var response = await _classUnderTest.Get(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
        }

        [Test]
        public async Task GivenValidRequestWithAcceptTextCsvHeader_ThenReturnsListOfAssetOutputModel()
        {
            //arrange
            var assetOutputModel = new AssetOutputModel(TestData.Domain.GenerateAsset());
            assetOutputModel.Id = Faker.GlobalUniqueIndex;
            assetOutputModel.SchemeId = Faker.GlobalUniqueIndex + 1;
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<SearchAssetRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SearchAssetResponse
                {
                    Assets = new List<AssetOutputModel> { assetOutputModel }
                });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));
            var request = new SearchAssetApiRequest
            {
                SchemeId = assetOutputModel.SchemeId,
                AssetRegisterVersionId = 1,
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetOutputModel>>();
        }

        [TestCase(null, null, 1, 1, 1)]
        [TestCase(0, null, 1, 1, 1)]
        [TestCase(-1, null, 1, 1, 1)]
        [TestCase(null, "", 1, 1, 1)]
        [TestCase(null, " ", 1, 1, 1)]
        [TestCase(1, "address", -1, 1, 1)]
        [TestCase(1, "address", 0, 1, 1)]
        [TestCase(1, "address", 1, -1, 1)]
        [TestCase(1, "address", 1, 0, 1)]
        [TestCase(1, "address", 1, 1, null)]
        public void GivenInvalidRequest_ThenIsInvalid(int? schemeId, string address, int? page,
            int? pageSize, int? assetRegisterVersionId)
        {
            SearchAssetApiRequest apiRequest = new SearchAssetApiRequest
            {
                SchemeId = schemeId,
                Address = address,
                Page = page,
                PageSize = pageSize,
                AssetRegisterVersionId = assetRegisterVersionId
            };

            apiRequest.IsValid().Should().BeFalse();
        }

        [TestCase(1, null, 1, 1, 1)]
        [TestCase(2, null, 1, 1, 1)]
        [TestCase(3, null, 1, 1, 1)]
        [TestCase(null, "d", 1, 1, 1)]
        [TestCase(null, "e", 1, 1, 1)]
        [TestCase(null, "t", 1, 1, 1)]
        [TestCase(1, "a", 1, 1, 1)]
        [TestCase(2, "b", 2, 3, 1)]
        [TestCase(3, "c", 3, 5, 1)]
        [TestCase(1, "address", null, 1, 1)]
        [TestCase(1, "address", 1, null, 1)]
        [TestCase(1, "address", null, null, 1)]
        public void GivenValidRequest_ThenIsValid(int? schemeId, string address, int? page,
            int? pageSize, int? assetRegisterVersionId)
        {
            SearchAssetApiRequest apiRequest = new SearchAssetApiRequest
            {
                SchemeId = schemeId,
                Address = address,
                Page = page,
                PageSize = pageSize,
                AssetRegisterVersionId = assetRegisterVersionId
            };

            apiRequest.IsValid().Should().BeTrue();
        }

    }
}
