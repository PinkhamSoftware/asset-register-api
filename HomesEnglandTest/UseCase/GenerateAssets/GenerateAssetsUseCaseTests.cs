using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.UseCase.BulkCreateAsset;
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
        private Mock<IBulkCreateAssetUseCase> _mockUseCase;

        [SetUp]
        public void Setup()
        {
            _mockUseCase = new Mock<IBulkCreateAssetUseCase>();
            
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
            var list = new List<CreateAssetResponse>();
            for (int i = 0; i < recordCount; i++)
            {
                list.Add(new CreateAssetResponse());
            }
            _mockUseCase
                .Setup(s => s.ExecuteAsync(It.IsAny<IList<CreateAssetRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);
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
            _mockUseCase.Verify(s=> s.ExecuteAsync(It.IsAny<IList<CreateAssetRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
