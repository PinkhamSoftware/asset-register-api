using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.UseCase.GetAssetDevelopers;
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
        private readonly Mock<IGetAssetDevelopersUseCase> _mockDevelopersUseCase;

        public SearchFiltersControllerTests()
        {
            _mockUseCase = new Mock<IGetAssetRegionsUseCase>();
            _mockDevelopersUseCase = new Mock<IGetAssetDevelopersUseCase>();
            _classUnderTest = new SearchFiltersController(_mockUseCase.Object, _mockDevelopersUseCase.Object);
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
            var response = await _classUnderTest.GetRegions();
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
            await _classUnderTest.GetRegions();
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

            SetCSVContentType();
            //act
            var response = await _classUnderTest.GetRegions().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetRegion>>();
        }

        [Test]
        public async Task GivenValidRequest_ThenReturnsGetDevelopersResponse()
        {
            //arrange
            _mockDevelopersUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetDevelopersResponse
                {
                    Developers = new List<AssetDeveloper>
                    {
                        new AssetDeveloper()
                    }
                });
            //act
            var response = await _classUnderTest.GetDevelopers();
            //assert
            response.Should().NotBeNull();
        }

        [Test]
        public async Task GivenValidRequest_ThenCallsGetDevelopersUseCase()
        {
            //arrange
            _mockDevelopersUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetDevelopersResponse
                {
                    Developers = new List<AssetDeveloper>
                    {
                        new AssetDeveloper()
                    }
                });
            //act
            await _classUnderTest.GetDevelopers();
            //assert
            _mockDevelopersUseCase.Verify(v => v.ExecuteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GivenValidRequestWithAcceptTextCsvHeader_ThenReturnsListOfAssetDevelopers()
        {
            //arrange
            _mockDevelopersUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAssetDevelopersResponse
                {
                    Developers = new List<AssetDeveloper>
                    {
                        new AssetDeveloper()
                    }
                });

            SetCSVContentType();
            //act
            var response = await _classUnderTest.GetDevelopers().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetDeveloper>>();
        }

        private void SetCSVContentType()
        {
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(
                new KeyValuePair<string, StringValues>("accept", "text/csv"));
        }
    }
}
