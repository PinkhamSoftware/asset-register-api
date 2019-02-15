using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Assets.Region;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.GetAssetRegions.Impl;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.GetAssetRegions
{

    [TestFixture]
    public class GetAssetRegionsUseCaseTests
    {
        private IGetAssetRegionsUseCase _classUnderTest;
        private Mock<IAssetRegionLister> _mockGateway;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IAssetRegionLister>();
            _classUnderTest = new GetAssetRegionsUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task WhenWeExecute_ThenWeCallGateways()
        {
            //arrange
            _mockGateway.Setup(s => s.ListRegionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<AssetRegion>());
            //act
            var response = await _classUnderTest.ExecuteAsync(CancellationToken.None)
                .ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            _mockGateway.Verify(v => v.ListRegionsAsync(It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GivenTheGatewayReturnsResults_WhenWeExecute_ThenTheResultsAreReturned()
        {
            //arrange
            _mockGateway.Setup(s => s.ListRegionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<AssetRegion>
            {
                new AssetRegion(),
                new AssetRegion()
            });
            //act
            var response = await _classUnderTest.ExecuteAsync(CancellationToken.None)
                .ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Regions.Count.Should().Be(2);
        }
    }
}
