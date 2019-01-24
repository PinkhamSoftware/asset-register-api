using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Impl;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Moq;
using NUnit.Framework;
using TestHelper;

namespace HomesEnglandTest.UseCase.CalculateAssetAggregates
{
    [TestFixture]
    public class CalculateAssetAggregatesTests
    {
        private readonly ICalculateAssetAggregatesUseCase _classUnderTest;
        private readonly Mock<IAssetAggregator> _mockGateway;

        public CalculateAssetAggregatesTests()
        {
            _mockGateway = new Mock<IAssetAggregator>();
            _classUnderTest = new CalculateAssetAggregatesUseCase(_mockGateway.Object);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(null)]
        public async Task GivenValidSchemeId_UseCaseCallsGatewayWithCorrectId(int? id)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            asset.SchemeId = id;
            _mockGateway.Setup(s=> s.Aggregate(It.IsAny<IAssetSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AssetAggregation());
            //act
            await _classUnderTest.ExecuteAsync(new CalculateAssetAggregateRequest
            {
                SchemeId = id
            }, CancellationToken.None);
            //assert
            _mockGateway.Verify(v=> v.Aggregate(It.Is<AssetSearchQuery>(req => req.SchemeId == id), It.IsAny<CancellationToken>()));
        }

        [TestCase("address1")]
        [TestCase("address2")]
        [TestCase("address3")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public async Task GivenValidAddress_UseCaseCallsGatewayWithCorrectAddress(string address)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Setup(s => s.Aggregate(It.IsAny<IAssetSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AssetAggregation());
            //act
            await _classUnderTest.ExecuteAsync(new CalculateAssetAggregateRequest
            {
                Address = address,
                AssetRegisterVersionId = 1
            }, CancellationToken.None);
            //assert
            _mockGateway.Verify(v => v.Aggregate(It.Is<AssetSearchQuery>(req => req.Address == address), It.IsAny<CancellationToken>()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenValidAssetRegisterVersion_UseCaseCallsGatewayWithCorrectAddress(int assetRegisterVersionId)
        {
            //arrange
            _mockGateway.Setup(s => s.Aggregate(It.IsAny<IAssetSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AssetAggregation());
            //act
            await _classUnderTest.ExecuteAsync(new CalculateAssetAggregateRequest
            {
                AssetRegisterVersionId = assetRegisterVersionId
            }, CancellationToken.None);
            //assert
            _mockGateway.Verify(v => v.Aggregate(It.Is<AssetSearchQuery>(req => req.AssetRegisterVersionId.Equals(assetRegisterVersionId)), It.IsAny<CancellationToken>()));
        }


        [TestCase(1, 2.0, 3.0, 4.0)]
        [TestCase(3, 4.0, 5.0, 6.0)]
        [TestCase(5, 6.0, 7.0, 8.0)]
        [TestCase(0, 0.0, 0.0, 0.0)]
        public async Task GivenValidInput_WhenGatewayReturnsResults_ThenUseCaseReturnsCorrectlyMappedResponse(int uniqueRecords, decimal moneyPaidOut, decimal assetValue, decimal movementInAssetValue)
        {
            _mockGateway.Setup(s => s.Aggregate(It.IsAny<IAssetSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync( new AssetAggregation
                {
                    UniqueRecords = uniqueRecords, 
                    MoneyPaidOut = moneyPaidOut,
                    AssetValue = assetValue,
                    MovementInAssetValue = movementInAssetValue
                });
            var request = new CalculateAssetAggregateRequest
            {
                AssetRegisterVersionId = 1,
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.AssetAggregates.Should().NotBeNull();
            response.AssetAggregates.UniqueRecords.Should().Be(uniqueRecords);
            response.AssetAggregates.MoneyPaidOut.Should().Be(moneyPaidOut);
            response.AssetAggregates.AssetValue.Should().Be(assetValue);
            response.AssetAggregates.MovementInAssetValue.Should().Be(movementInAssetValue);
        }
    }
}
