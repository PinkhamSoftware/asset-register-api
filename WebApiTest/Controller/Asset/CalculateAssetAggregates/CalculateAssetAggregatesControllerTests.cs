using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.BulkCreateAsset.Models;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.Search.Calculations;
using WebApi.Extensions;
using WebApi.Extensions.Requests;

namespace WebApiTest.Controller.Asset.CalculateAssetAggregates
{
    [TestFixture]
    public class CalculateAssetAggregatesControllerTests
    {
        private readonly CalculateAssetAggregatesController _classUnderTest;
        private readonly Mock<ICalculateAssetAggregatesUseCase> _mockUseCase;
        private readonly Mock<IAssetRegisterVersionSearcher> _mockAssetRegisterVersionSearcher;

        public CalculateAssetAggregatesControllerTests()
        {
            _mockUseCase = new Mock<ICalculateAssetAggregatesUseCase>();
            _mockAssetRegisterVersionSearcher = new Mock<IAssetRegisterVersionSearcher>();
            _classUnderTest = new CalculateAssetAggregatesController(_mockUseCase.Object, _mockAssetRegisterVersionSearcher.Object);
        }

        [TestCase(1, 100.01, 200.02, 100.01)]
        [TestCase(2, 100.01, 200.02, 100.01)]
        [TestCase(3, 100.01, 200.02, 100.01)]
        public async Task GivenValidRequest_ThenReturnsAssetAggregationResponse(int uniqueRecords, decimal? agencyEquityValue, decimal? agencyFairValue, decimal? movementInFairValue)
        {
            //arrange
            var assetAggregatesOutputModel = new AssetAggregatesOutputModel
            {
                UniqueRecords = uniqueRecords,
                MoneyPaidOut = agencyEquityValue,
                AssetValue = agencyFairValue,
                MovementInAssetValue = movementInFairValue
            };
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CalculateAssetAggregateRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CalculateAssetAggregateResponse
                {
                    AssetAggregates = assetAggregatesOutputModel
                });

            var request = new CalculateAssetAggregateRequest();
            //act
            var response = await _classUnderTest.Get(request);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<ResponseData<CalculateAssetAggregateResponse>>();
            var apiResponse = result.Value as ResponseData<CalculateAssetAggregateResponse>;
            response.Should().NotBeNull();
            apiResponse.Data.AssetAggregates.Should().BeEquivalentTo(assetAggregatesOutputModel);
        }

        [TestCase(1, 100.01, 200.02, 100.01)]
        [TestCase(2, 100.01, 200.02, 100.01)]
        [TestCase(3, 100.01, 200.02, 100.01)]
        public async Task GivenValidRequestWithAcceptTextCsvHeader_ThenReturnsListOfAssetAggregatesOutputModel(int uniqueRecords, decimal? agencyEquityValue, decimal? agencyFairValue, decimal? movementInFairValue)
        {
            //arrange
            var assetAggregatesOutputModel = new AssetAggregatesOutputModel
            {
                UniqueRecords = uniqueRecords,
                MoneyPaidOut = agencyEquityValue,
                AssetValue = agencyFairValue,
                MovementInAssetValue = movementInFairValue
            };
            _mockUseCase.Setup(s => s.ExecuteAsync(It.Is<CalculateAssetAggregateRequest>(a=> a.Address.Equals("test")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CalculateAssetAggregateResponse
                {
                    AssetAggregates = assetAggregatesOutputModel
                });
            _classUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _classUnderTest.ControllerContext.HttpContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("accept", "text/csv"));

            var request = new CalculateAssetAggregateRequest
            {
                Address = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            var result = response as ObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<List<AssetAggregatesOutputModel>>();
            var list = result.Value as List<AssetAggregatesOutputModel>;
            list.Should().NotBeNullOrEmpty();
            list.ElementAtOrDefault(0).Should().BeEquivalentTo(assetAggregatesOutputModel);
        }

        [Test]
        public async Task GivenRequestWithoutAssetRegisterVersion_ThenCallsUseCaseWithMostRecentVersionOfAssetRegister()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CalculateAssetAggregateRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CalculateAssetAggregateResponse());
            _mockAssetRegisterVersionSearcher
                .Setup(s => s.Search(It.IsAny<PagedQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                    new PagedResults<IAssetRegisterVersion>
                    {
                        Results = new List<IAssetRegisterVersion>
                        {
                            new AssetRegisterVersion
                            {
                                Id = 10
                            }
                        },
                        TotalCount = 1,
                        NumberOfPages = 1
                    });
            var request = new CalculateAssetAggregateRequest
            {
                Address = "test",
            };

            //act
            await _classUnderTest.Get(request);
            //assert
            _mockUseCase.Verify(v => v.ExecuteAsync(It.Is<CalculateAssetAggregateRequest>(i => i.AssetRegisterVersionId.Equals(10)), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GivenRequestWithoutAssetRegisterVersion_ThenCallsAssetRegisterVersionSearcher_WithCorrectParams()
        {
            //arrange
            _mockUseCase.Setup(s => s.ExecuteAsync(It.IsAny<CalculateAssetAggregateRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CalculateAssetAggregateResponse());
            _mockAssetRegisterVersionSearcher
                .Setup(s => s.Search(It.IsAny<PagedQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                    new PagedResults<IAssetRegisterVersion>
                    {
                        Results = new List<IAssetRegisterVersion>
                        {
                            new AssetRegisterVersion
                            {
                                Id = 10
                            }
                        },
                        TotalCount = 1,
                        NumberOfPages = 1
                    });

            var request = new CalculateAssetAggregateRequest
            {
                Address = "test",
            };

            //act
            await _classUnderTest.Get(request);
            //assert
            _mockAssetRegisterVersionSearcher.Verify(v => v.Search(It.Is<PagedQuery>(i => i.Page.Equals(1) && i.PageSize.Equals(1)), It.IsAny<CancellationToken>()));
        }
    }
}
