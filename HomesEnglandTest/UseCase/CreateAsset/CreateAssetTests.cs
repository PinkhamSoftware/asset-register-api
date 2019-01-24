using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Exception;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Impl;
using HomesEngland.UseCase.GetAsset.Models;
using Moq;
using NUnit.Framework;
using TestHelper;

namespace HomesEnglandTest.UseCase.CreateAsset
{
    public class CreateAssetTests
    {

        private readonly ICreateAssetUseCase _classUnderTest;
        private readonly Mock<IAssetCreator> _gateway;

        public CreateAssetTests()
        {
            _gateway = new Mock<IAssetCreator>();
            
            _classUnderTest = new CreateAssetUseCase(_gateway.Object);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public async Task GivenValidRequest_UseCaseCallsGatewayWithCorrectDomainObject(int schemeId, int createdAssetId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            _gateway.Setup(s => s.CreateAsync(It.IsAny<IAsset>())).ReturnsAsync(new Asset(request));
            //act
            var useCaseResponse = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _gateway.Verify(s=> s.CreateAsync(It.Is<IAsset>(i=> i.AssetIsEqual(request))));
            useCaseResponse.Should().NotBeNull();
            useCaseResponse.Asset.Should().NotBeNull();
            useCaseResponse.Asset.AssetOutputModelIsEqual(request);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public async Task GivenValidRequest_UseCaseReturnsAssetOutputModel(int schemeId, int createdAssetId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            _gateway.Setup(s => s.CreateAsync(It.IsAny<IAsset>())).ReturnsAsync(new Asset(request));
            //act
            var useCaseResponse = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            useCaseResponse.Should().NotBeNull();
            useCaseResponse.Asset.Should().NotBeNull();
            useCaseResponse.Asset.AssetOutputModelIsEqual(request);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public void GivenValidRequest_WhenGatewayReturnsNull_ThenUseCaseThrowsAssetNotCreatedException(int schemeId, int createdAssetId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            _gateway.Setup(s => s.CreateAsync(It.IsAny<IAsset>())).ReturnsAsync((IAsset)null);
            //act
            //assert
            Assert.ThrowsAsync<CreateAssetException>(async ()=> await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
        }
    }
}
