using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain.Factory;
using HomesEngland.UseCase.BulkCreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.ImportAssets.Models;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.ImportAssets
{
    [TestFixture]
    public class ImportAssetsUseCaseTest
    {
        public IImportAssetsUseCase _classUnderTest;
        public Mock<IBulkCreateAssetUseCase> _mockBulkCreateAssetUseCase;
        public Mock<IFactory<CreateAssetRequest, CsvAsset>> _mockCreateAssetFactory;

        [SetUp]
        public void Setup()
        {
            _mockBulkCreateAssetUseCase = new Mock<IBulkCreateAssetUseCase>();
            _mockCreateAssetFactory = new Mock<IFactory<CreateAssetRequest, CsvAsset>>();
            _classUnderTest = new ImportAssetsUseCase(_mockBulkCreateAssetUseCase.Object, _mockCreateAssetFactory.Object);
        }

        private void StubCreateAssetUseCase()
        {
            var createAssetResponse = new CreateAssetResponse
            {
                Asset = new AssetOutputModel
                {
                    Address = "Test"
                }
            };

            _mockBulkCreateAssetUseCase
                .Setup(s => s.ExecuteAsync(It.IsAny<IList<CreateAssetRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CreateAssetResponse>{createAssetResponse});
        }

        private void StubCreateAssetUseCaseWithAddress(string address, string address2)
        {
            var assetResponse = new CreateAssetResponse
            {
                Asset = new AssetOutputModel
                {
                    Address = address
                }
            };

            var assetResponse2 = new CreateAssetResponse
            {
                Asset = new AssetOutputModel
                {
                    Address = address2
                }
            };

            _mockBulkCreateAssetUseCase
                .Setup(s => s.ExecuteAsync(
                    It.Is<IList<CreateAssetRequest>>(req => req[0].Address.Equals(address) || req[1].Address.Equals(address2)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CreateAssetResponse> { assetResponse, assetResponse2 });
        }

        private void StubFactoryWithAddress(string csvLine, string address)
        {
            var factoryResponse = new CreateAssetRequest
            {
                Address = address
            };

            _mockCreateAssetFactory.Setup(s => s.Create(It.Is<CsvAsset>(req => req.CsvLine.Equals(csvLine))))
                .Returns(factoryResponse);
        }


        public class GivenSingleInput : ImportAssetsUseCaseTest
        {
            [TestCase("Meow")]
            [TestCase("Woof")]
            public async Task ThenCallTheFactoryWithTheCorrectInput(string input)
            {
                StubCreateAssetUseCase();

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {input}
                };

                await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                _mockCreateAssetFactory.Verify(v => v.Create(It.Is<CsvAsset>(req => req.CsvLine == input)));
            }

            [TestCase(";")]
            [TestCase(",")]
            public async Task ThenCallTheFactoryWithTheCorrectDelimiter(string delimiter)
            {
                StubCreateAssetUseCase();

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {"Test"},
                    Delimiter = delimiter
                };

                await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                _mockCreateAssetFactory.Verify(v => v.Create(It.Is<CsvAsset>(req => req.Delimiter == delimiter)));
            }

            [TestCase("Meow")]
            [TestCase("Woof")]
            public async Task ThenCallTheCreateAssetUseCaseWithTheResultFromTheFactory(string input)
            {
                StubCreateAssetUseCase();
                StubFactoryWithAddress(input, input);

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {input}
                };

                await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                _mockBulkCreateAssetUseCase.Verify(v =>
                    v.ExecuteAsync(It.Is<IList<CreateAssetRequest>>(req => req[0].Address.Equals(input)),
                        It.IsAny<CancellationToken>()));
            }

            [TestCase("Meow")]
            [TestCase("Woof")]
            public async Task ThenReturnTheCreatedAssets(string input)
            {
                var createAssetResponse = new CreateAssetResponse
                {
                    Asset = new AssetOutputModel
                    {
                        Address = input
                    }
                };

                _mockBulkCreateAssetUseCase
                    .Setup(s => s.ExecuteAsync(It.IsAny<IList<CreateAssetRequest>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<CreateAssetResponse>{createAssetResponse});

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {input}
                };

                var results = await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                var createdAsset = results.AssetsImported[0];

                createdAsset.Address.Should().BeEquivalentTo(input);
            }
        }

        public class GivenTwoLines : ImportAssetsUseCaseTest
        {
            [TestCase("Meow", "Woof")]
            public async Task ThenWeCallTheFactoryWithTheInputs(string inputOne, string inputTwo)
            {
                StubCreateAssetUseCase();

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {inputOne, inputTwo}
                };

                await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                _mockCreateAssetFactory.Verify(v => v.Create(It.Is<CsvAsset>(req => req.CsvLine == inputOne)));
                _mockCreateAssetFactory.Verify(v => v.Create(It.Is<CsvAsset>(req => req.CsvLine == inputTwo)));
            }

            [TestCase("Meow", "Woof")]
            public async Task ThenWeReturnAListOfTheCreatedAssets(string inputOne, string inputTwo)
            {
                StubCreateAssetUseCaseWithAddress(inputOne,inputTwo);
                StubFactoryWithAddress(inputOne, inputOne);
                StubFactoryWithAddress(inputTwo, inputTwo);

                var request = new ImportAssetsRequest
                {
                    AssetLines = new List<string> {inputOne, inputTwo}
                };

                var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None).ConfigureAwait(false);

                var createdAssets = response.AssetsImported;

                createdAssets[0].Address.Should().BeEquivalentTo(inputOne);
                createdAssets[1].Address.Should().BeEquivalentTo(inputTwo);
            }
        }
    }
}
