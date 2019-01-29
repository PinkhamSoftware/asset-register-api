using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApiTest.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostTests
    {
        private AssetRegisterVersionController _classUnderTest;
        private Mock<ISaveUploadedAssetRegisterFileUseCase> _mockUseCase;
        private Mock<IGetAssetRegisterVersionsUseCase> _mockGetUseCase;

        [SetUp]
        public void  Setup()
        {
            _mockUseCase = new Mock<ISaveUploadedAssetRegisterFileUseCase>();
            _mockGetUseCase = new Mock<IGetAssetRegisterVersionsUseCase>();
            _classUnderTest = new AssetRegisterVersionController(_mockGetUseCase.Object,_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidFile_WhenUploading_ThenCanSaveFile()
        {
            //arrange
            var memoryStream = new MemoryStream();
            //act
            _classUnderTest.Post(new List<IFormFile>{new FormFile(memoryStream,0,)})
            //asset

        }
    }
}
