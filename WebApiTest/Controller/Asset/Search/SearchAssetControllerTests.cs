using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using TestHelper;
using WebApi.Controllers.Search;
using WebApi.Extensions.Requests;

namespace WebApiTest.Controller.Asset.Search
{
    [TestFixture]
    public class SearchAssetControllerTests
    {
        private readonly SearchAssetController _classUnderTest;
        private readonly Mock<ISearchAssetUseCase> _mockUseCase;

        public SearchAssetControllerTests()
        {
            _mockUseCase = new Mock<ISearchAssetUseCase>();
            _classUnderTest = new SearchAssetController(_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidRequest_ThenReturnsGetAssetResponse()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<SearchAssetRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SearchAssetResponse());
            var request = new SearchAssetApiRequest();
            //act
            var response = await _classUnderTest.Get(request);
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
                    Assets = new List<AssetOutputModel> {assetOutputModel}
                });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));
            var request = new SearchAssetApiRequest
            {
                SchemeId = assetOutputModel.SchemeId
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetOutputModel>>();
        }

        [TestCase(null, null, 1, 1)]
        [TestCase(0, null, 1, 1)]
        [TestCase(-1, null, 1, 1)]
        [TestCase(null, "", 1, 1)]
        [TestCase(null, " ", 1, 1)]
        [TestCase(1, "address", -1, 1)]
        [TestCase(1, "address", 0, 1)]
        [TestCase(1, "address", 1, -1)]
        [TestCase(1, "address", 1, 0)]
        public void GivenInvalidRequest_ThenIsInvalid(int? schemeId, string address, int? page,
            int? pageSize)
        {
            SearchAssetApiRequest apiRequest = new SearchAssetApiRequest
            {
                SchemeId = schemeId,
                Address = address,
                Page = page,
                PageSize = pageSize
            };
            
            apiRequest.IsValid().Should().BeFalse();
        }
        
        [TestCase(1, null, 1, 1)]
        [TestCase(2, null, 1, 1)]
        [TestCase(3, null, 1, 1)]
        [TestCase(null, "d", 1, 1)]
        [TestCase(null, "e", 1, 1)]
        [TestCase(null, "t", 1, 1)]
        [TestCase(1, "a", 1, 1)]
        [TestCase(2, "b", 2, 3)]
        [TestCase(3, "c", 3, 5)]
        [TestCase(1, "address", null, 1)]
        [TestCase(1, "address", 1, null)]
        [TestCase(1, "address", null, null)]
        public void GivenValidRequest_ThenIsValid(int? schemeId, string address, int? page,
            int? pageSize)
        {
            SearchAssetApiRequest apiRequest = new SearchAssetApiRequest
            {
                SchemeId = schemeId,
                Address = address,
                Page = page,
                PageSize = pageSize
            };

            apiRequest.IsValid().Should().BeTrue();
        }

    }
}
