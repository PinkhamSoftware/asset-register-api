using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets.Developer;
using HomesEngland.UseCase.GetAssetDevelopers;
using HomesEngland.UseCase.GetAssetDevelopers.Impl;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.GetAssetDevelopers
{ 
    [TestFixture]
    public class GetAssetDevelopersUseCaseTests
    {
        private IGetAssetDevelopersUseCase _classUnderTest;
        private Mock<IAssetDeveloperLister> _mockGateway;

        [SetUp]
        public void Setup()
        {
            _mockGateway = new Mock<IAssetDeveloperLister>();
            _classUnderTest = new GetAssetDevelopersUseCase(_mockGateway.Object);
        }

        [Test]
        public async Task WhenWeExecute_ThenWeCallGateways()
        {
            //arrange
            _mockGateway.Setup(s => s.ListDevelopersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<AssetDeveloper>());
            //act
            var response = await _classUnderTest.ExecuteAsync(CancellationToken.None)
                .ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            _mockGateway.Verify(v => v.ListDevelopersAsync(It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GivenTheGatewayReturnsResults_WhenWeExecute_ThenTheResultsAreReturned()
        {
            //arrange
            _mockGateway.Setup(s => s.ListDevelopersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<AssetDeveloper>
            {
                new AssetDeveloper(),
                new AssetDeveloper()
            });
            //act
            var response = await _classUnderTest.ExecuteAsync(CancellationToken.None)
                .ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Developers.Count.Should().Be(2);
        }
    }
}
