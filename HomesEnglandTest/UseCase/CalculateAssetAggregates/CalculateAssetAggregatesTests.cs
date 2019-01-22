using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Impl;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using NSubstitute;
using NUnit.Framework;
using TestHelper;

namespace HomesEnglandTest.UseCase.CalculateAssetAggregates
{
    [TestFixture]
    public class CalculateAssetAggregatesTests
    {
        private readonly ICalculateAssetAggregatesUseCase _classUnderTest;
        private readonly IAssetAggregator _mockGateway;

        public CalculateAssetAggregatesTests()
        {
            _mockGateway = Substitute.For<IAssetAggregator>();
            _classUnderTest = new CalculateAssetAggregatesUseCase(_mockGateway);
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
            _mockGateway.Aggregate(Arg.Any<IAssetSearchQuery>(), CancellationToken.None)
                .Returns(new AssetAggregation());
            //act
            await _classUnderTest.ExecuteAsync(new CalculateAssetAggregateRequest
            {
                SchemeId = id
            }, CancellationToken.None);
            //assert
            await _mockGateway.Received()
                .Aggregate(Arg.Is<AssetSearchQuery>(req => req.SchemeId == id), Arg.Any<CancellationToken>());
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
            _mockGateway.Aggregate(Arg.Any<IAssetSearchQuery>(), CancellationToken.None)
                .Returns(new AssetAggregation());
            //act
            await _classUnderTest.ExecuteAsync(new CalculateAssetAggregateRequest
            {
                Address = address
            }, CancellationToken.None);
            //assert
            await _mockGateway.Received()
                .Aggregate(Arg.Is<AssetSearchQuery>(req => req.Address == address), Arg.Any<CancellationToken>());
        }


        [TestCase(1, 2.0, 3.0, 4.0)]
        [TestCase(3, 4.0, 5.0, 6.0)]
        [TestCase(5, 6.0, 7.0, 8.0)]
        [TestCase(0, 0.0, 0.0, 0.0)]
        public async Task GivenValidInput_WhenGatewayReturnsResults_ThenUseCaseReturnsCorrectlyMappedResponse(int uniqueRecords, decimal moneyPaidOut, decimal assetValue, decimal movementInAssetValue)
        {
            _mockGateway.Aggregate(Arg.Any<IAssetSearchQuery>(), CancellationToken.None)
                .Returns( new AssetAggregation
                {
                    UniqueRecords = uniqueRecords, 
                    MoneyPaidOut = moneyPaidOut,
                    AssetValue = assetValue,
                    MovementInAssetValue = movementInAssetValue
                });
            //act
            var response = await _classUnderTest.ExecuteAsync(null, CancellationToken.None);
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
