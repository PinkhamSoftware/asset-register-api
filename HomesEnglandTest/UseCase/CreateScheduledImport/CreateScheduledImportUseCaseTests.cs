using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.CreateScheduledImport;
using HomesEngland.UseCase.CreateScheduledImport.Impl;
using HomesEngland.UseCase.CreateScheduledImport.Models;
using HomesEngland.UseCase.SaveFile;
using HomesEngland.UseCase.SaveFile.Model;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.CreateScheduledImport
{
    [TestFixture]
    public class CreateScheduledImportUseCaseTests
    {
        private ICreateScheduledImportUseCase _classUnderTest;
        private Mock<ISaveAssetRegisterFileUseCase> _mockSaveFileUseCase;
        [SetUp]
        public void Setup()
        {
            _mockSaveFileUseCase = new Mock<ISaveAssetRegisterFileUseCase>();
            _classUnderTest = new CreateScheduledImportUseCase(_mockSaveFileUseCase.Object);
        }

        [TestCase("test.csv", "text")]
        [TestCase("test2.csv", "text2")]
        [TestCase("test3.csv", "text3")]
        public async Task GivenValidInput_WhenExecuting_ThenReturnsId(string fileName, string text)
        {
            //arrange
            _mockSaveFileUseCase
                .Setup(s => s.ExecuteAsync(It.IsAny<SaveAssetRegisterFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SaveAssetRegisterFileResponse
                {
                    Id = 1
                });
            //act
            var response = await _classUnderTest.ExecuteAsync(new CreateScheduledImportRequest
            {
                FileName = fileName,
                Text = text
            }, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();

            response.Id.Should().BeGreaterThan(0);
        }

        [TestCase("test.csv", "text")]
        [TestCase("test2.csv", "text2")]
        [TestCase("test3.csv", "text3")]
        public async Task GivenValidInput_WhenExecuting_ThenCallsSaveFileUse(string fileName, string text)
        {
            //arrange

            //act
            var response = await _classUnderTest.ExecuteAsync(new CreateScheduledImportRequest
            {
                FileName = fileName,
                Text = text
            }, CancellationToken.None).ConfigureAwait(false);
            //assert
            _mockSaveFileUseCase.Verify(v=> v.ExecuteAsync(It.Is<SaveAssetRegisterFileRequest>(i=> i.FileName == fileName && i.Text == text), It.IsAny<CancellationToken>()));
        }
    }
}
