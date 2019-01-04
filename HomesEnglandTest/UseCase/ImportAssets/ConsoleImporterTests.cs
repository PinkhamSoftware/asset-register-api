using System;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Impl;
using HomesEngland.UseCase.ImportAssets.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using HomesEngland.UseCase.Models;

namespace HomesEnglandTest.UseCase.ImportAssets
{
    [TestFixture]
    public class ConsoleImporterTests
    {
        private IConsoleImporter _classUnderTest;
        private Mock<ILogger<IConsoleImporter>> _mockLogger;
        private Mock<IImportAssetsUseCase> _mockImportAssetUseCase;
        private IInputParser<ImportAssetConsoleInput> _inputParser;
        private Mock<IFileReader<string>> _mockFileReader;
        private ITextSplitter _textSplitter;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<IConsoleImporter>>();
            _mockImportAssetUseCase = new Mock<IImportAssetsUseCase>();
            _inputParser = new ImportAssetInputParser();
            _mockFileReader = new Mock<IFileReader<string>>();
            _textSplitter = new TextSplitter();
            _classUnderTest = new ConsoleImporter(_mockImportAssetUseCase.Object, _inputParser, _mockFileReader.Object,_textSplitter, _mockLogger.Object);
        }

        [TestCase("--file", "test.csv", "--delimiter", ";")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ";")]
        [TestCase("--file", "Test.csv", "--delimiter", ";")]
        public async Task GivenWeNeedToImportAssets_WhenWeDoSoThroughAConsoleInterface_ThenWeCallImportAssetsUseCase(string arg1, string arg2, string arg3, string arg4)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockImportAssetUseCase.Verify(s => s.ExecuteAsync(It.IsAny<ImportAssetsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase("--file", "test.csv", "--delimiter", ";")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ",")]
        [TestCase("--file", "Test.csv", "--delimiter", "\t")]
        public async Task GivenValidInput_WhenWeRunTheGenerator_ThenTheArgumentsArePassedIn(string arg1, string arg2, string arg3, string arg4)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockImportAssetUseCase.Verify(s => s.ExecuteAsync(It.Is<ImportAssetsRequest>(i => i.Delimiter.Equals(arg4)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase("--file", "test.csv", "--delimiter", ";")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ",")]
        [TestCase("--file", "Test.csv", "--delimiter", "\t")]
        public async Task GivenValidInput_WhenWeRunTheGenerator_ThenTheInputParserIsCalled(string arg1, string arg2, string arg3, string arg4)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockImportAssetUseCase.Verify(s => s.ExecuteAsync(It.Is<ImportAssetsRequest>(i => i.Delimiter.Equals(arg4)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void GivenInValidInput_WhenWeRunTheGenerator_ThenWeThrowAnException()
        {
            //arrange
            //act
            //assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _classUnderTest.ProcessAsync(null).ConfigureAwait(false));
        }

        [TestCase("--file", "test.csv", "--delimiter", ";")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ";")]
        [TestCase("--file", "Test.csv", "--delimiter", ";")]
        public async Task GivenWeNeedToImportAssets_WhenWeDoSoThroughAConsoleInterface_ThenWeCallFileReader(string arg1, string arg2, string arg3, string arg4)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockFileReader.Verify(s => s.ReadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase("--file", "test.csv", "--delimiter", ";")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ";")]
        [TestCase("--file", "Test.csv", "--delimiter", ";")]
        public async Task GivenWeNeedToImportAssets_WhenWeWantToReadFileContents_ThenWeCallFileReaderWithFilePath(string arg1, string arg2, string arg3, string arg4)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockFileReader.Verify(s => s.ReadAsync(It.Is<string>(i=> i.Equals(arg2)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase("--file", "test.csv", "--delimiter", ";",3, "Line1\nLine2\nLine3")]
        [TestCase("--file", "TEST.CSV", "--delimiter", ";",2, "Line1\nLine2")]
        [TestCase("--file", "Test.csv", "--delimiter", ";",1, "Line1")]
        public async Task GivenWeNeedToImportAssets_WhenWeWantParse_Then(string arg1, string arg2, string arg3, string arg4, int expectedLineCount, string csvText)
        {
            //arrange
            var args = new[] { arg1, arg2, arg3, arg4 };
            _mockFileReader.Setup(s => s.ReadAsync(arg2, It.IsAny<CancellationToken>())).ReturnsAsync(csvText);
            //act
            await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
            //assert
            _mockImportAssetUseCase.Verify(s => s.ExecuteAsync(It.Is<ImportAssetsRequest>(i=> i.AssetLines.Count.Equals(expectedLineCount)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
