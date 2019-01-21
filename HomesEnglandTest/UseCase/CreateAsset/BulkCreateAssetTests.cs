using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Exception;
using HomesEngland.Gateway;
using HomesEngland.UseCase.BulkCreateAsset;
using HomesEngland.UseCase.BulkCreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models;
using Moq;
using NUnit.Framework;
using TestHelper;

namespace HomesEnglandTest.UseCase.CreateAsset
{
    public class BulkCreateAssetTests
    {

        private readonly IBulkCreateAssetUseCase _classUnderTest;
        private readonly Mock<IBulkAssetCreator> _gateway;

        public BulkCreateAssetTests()
        {
            _gateway = new Mock<IBulkAssetCreator>();

            _classUnderTest = new BulkCreateAssetUseCase(_gateway.Object);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public async Task GivenValidRequest_UseCaseCallsGatewayWithCorrectDomainObject(int schemeId, int createdAssetId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            _gateway.Setup(s => s.BulkCreateAsync(It.IsAny<IAssetRegisterVersion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IAsset>(){ new Asset(request)});

            var list = new List<CreateAssetRequest> {request};
            
            //act
            var useCaseResponse = await _classUnderTest.ExecuteAsync(list, CancellationToken.None);
            //assert
            _gateway.Verify(s => s.BulkCreateAsync(It.Is<IAssetRegisterVersion>(i => i.Assets[0].SchemeId.Equals(schemeId)), It.IsAny<CancellationToken>()));
            useCaseResponse.Should().NotBeNull();
            useCaseResponse[0].Asset.Should().NotBeNull();
            useCaseResponse[0].Asset.AssetOutputModelIsEqual(request);
        }

        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public async Task GivenValidRequest_UseCaseReturnsAssetOutputModels(int schemeId, int createdAssetId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            _gateway.Setup(s => s.BulkCreateAsync(It.IsAny<IAssetRegisterVersion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IAsset>() { new Asset(request) });
            var list = new List<CreateAssetRequest> {request};
            //act
            var useCaseResponse = await _classUnderTest.ExecuteAsync(list, CancellationToken.None);
            //assert
            useCaseResponse.Should().NotBeNull();
            useCaseResponse[0].Asset.Should().NotBeNull();
            useCaseResponse[0].Asset.AssetOutputModelIsEqual(request);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GivenValidRequest_WhenGatewayReturnsNull_ThenUseCaseThrowsAssetNotCreatedException(int schemeId)
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            request.SchemeId = schemeId;
            var list = new List<CreateAssetRequest> {request};
            _gateway.Setup(s => s.BulkCreateAsync(It.IsAny<IAssetRegisterVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync((IList<IAsset>)null);
            //act
            //assert
            Assert.ThrowsAsync<BulkCreateAssetException>(async () => await _classUnderTest.ExecuteAsync(list, CancellationToken.None));
        }

        [Test]
        public async Task GivenValidRequest_WhenUseCaseCallsGateway_ThenModifiedDateIsSet()
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            var list = new List<CreateAssetRequest> { request };
            _gateway.Setup(s => s.BulkCreateAsync(It.IsAny<IAssetRegisterVersion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IAsset>() { new Asset(request) });
            //act
            await _classUnderTest.ExecuteAsync(list, CancellationToken.None);
            //assert
            _gateway.Verify(s => s.BulkCreateAsync(It.Is<IAssetRegisterVersion>(i => i.ModifiedDateTime != DateTime.MinValue), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GivenValidRequest_WhenUseCaseCallsGateway_ThenAssetsAreSet()
        {
            //arrange
            var request = TestData.UseCase.GenerateCreateAssetRequest();
            var list = new List<CreateAssetRequest> { request };
            _gateway.Setup(s => s.BulkCreateAsync(It.IsAny<IAssetRegisterVersion>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IAsset>() { new Asset(request) });
            //act
            await _classUnderTest.ExecuteAsync(list, CancellationToken.None);
            //assert
            _gateway.Verify(s => s.BulkCreateAsync(It.Is<IAssetRegisterVersion>(i => i.Assets[0].AssetIsEqual(request)), It.IsAny<CancellationToken>()));
        }
    }
}
