using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using NSubstitute;
using NUnit.Framework;
using TestHelper;
using FluentAssertions;
using HomesEngland.Exception;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.GetAsset.Impl;
using Infrastructure.Api.Exceptions;

namespace HomesEnglandTest.UseCase.GetAsset
{
    [TestFixture]
    public class GetAssetTests
    {
        private readonly IGetAssetUseCase _classUnderTest;
        private readonly IAssetReader _mockGateway;
        public GetAssetTests()
        {
            _mockGateway = Substitute.For<IAssetReader>();
            _classUnderTest = new GetAssetUseCase(_mockGateway);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenValidRequestId_UseCaseReturnsCorrectlyMappedAsset(int id)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            asset.Id = id;
            _mockGateway.ReadAsync(id).Returns(asset);
            //act
            var response = await _classUnderTest.ExecuteAsync(new GetAssetRequest
            {
                Id = id
            });
            //assert
            response.Should().NotBeNull();
            response.Asset.Should().NotBeNull();
            response.Asset.Should().BeEquivalentTo(asset);
        }

        [Test]
        public void GivenValidRequestId_UseCaseReturnsCorrectlyMappedAsset()
        {
            //arrange
            //act
            //assert
            Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(null));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(null)]
        public void GivenInValidRequest_ThenUseCaseThrowsBadRequestException(int? id)
        {
            //arrange
            var getAssetRequest = new GetAssetRequest
            {
                Id = id
            };
            _mockGateway.ReadAsync(0).Returns((IAsset)null);
            //act
            //assert
            Assert.ThrowsAsync<BadRequestException>(async()=>await _classUnderTest.ExecuteAsync(getAssetRequest));
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void GivenValidRequest_WhenAssetCannotBeFound_ThenUseCaseThrowsAssetNotFoundException(int id)
        {
            //arrange
            var getAssetRequest = new GetAssetRequest
            {
                Id = id
            };
            _mockGateway.ReadAsync(id).Returns((IAsset)null);
            //act
            //assert
            Assert.ThrowsAsync<AssetNotFoundException>(async () => await _classUnderTest.ExecuteAsync(getAssetRequest));
        }
    }
}
