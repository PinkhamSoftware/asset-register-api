using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GenerateAssets.Impl;
using HomesEngland.UseCase.GenerateAssets.Models;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.GenerateAssets
{
    [TestFixture]
    public class GenerateAssetsUseCaseTest
    {
        private IGenerateAssetsUseCase _classUnderTest;
        private Mock<ICreateAssetUseCase> _mockUseCase;

        [SetUp]
        public void Setup()
        {
            _mockUseCase = new Mock<ICreateAssetUseCase>();
            _classUnderTest = new GenerateAssetsUseCase(_mockUseCase.Object);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GivenValidRequest_ThenUseCaseGeneratesCorrectNumberOfRecords(int recordCount)
        {
            //arrange 
            var request = new GenerateAssetsRequest
            {
                Records = recordCount
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.RecordsGenerated.Should().NotBeNull();
            response.RecordsGenerated.Count.Should().Be(recordCount);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GivenValidRequest_ThenUseCase_CallsCreateAssetUseCaseCorrectNumberOfTimes(int recordCount)
        {
            //arrange 
            var request = new GenerateAssetsRequest
            {
                Records = recordCount
            };
            //act
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            _mockUseCase.Verify(s=> s.ExecuteAsync(It.IsAny<CreateAssetRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(recordCount));
        }
    }
}
