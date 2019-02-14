using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.GetAssetRegions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.Search.Filters;

namespace WebApiTest.Controller.Asset.Search.Filters
{
    [TestFixture]
    public class SearchFiltersControllerTests
    {
        private readonly SearchFiltersController _classUnderTest;
        private readonly Mock<IGetAssetRegionsUseCase> _mockUseCase;
        

        public SearchFiltersControllerTests()
        {
            _mockUseCase = new Mock<IGetAssetRegionsUseCase>();
           
            _classUnderTest = new SearchFiltersController(_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidRequest_ThenReturnsGetAssetResponse()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetRegionsResponse
                {
                    Regions = new List<AssetRegion>
                    {
                        new AssetRegion()
                    }
                });
            //act
            var response = await _classUnderTest.Get();
            //assert
            response.Should().NotBeNull();
        }

        [Test]
        public async Task GivenValidRequest_ThenCallsUseCase()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetRegionsResponse
                {
                    Regions = new List<AssetRegion>
                    {
                        new AssetRegion()
                    }
                });
            //act
            await _classUnderTest.Get();
            //assert
            _mockUseCase.Verify(v=> v.ExecuteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GivenValidRequestWithAcceptTextCsvHeader_ThenReturnsListOfAssetRegions()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetRegionsResponse
                {
                    Regions = new List<AssetRegion>
                    {
                        new AssetRegion()
                    }
                });

            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("accept", "text/csv"));
            //act
            var response = await _classUnderTest.Get().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetRegion>>();
        }
    }
}
