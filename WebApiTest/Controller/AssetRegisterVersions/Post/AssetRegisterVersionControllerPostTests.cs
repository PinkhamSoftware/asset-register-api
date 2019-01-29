using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SaveUploadedAssetRegisterFile;
using Microsoft.AspNetCore.Http;
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
        private readonly AssetRegisterVersionController _classUnderTest;
        private readonly Mock<ISaveUploadedAssetRegisterFileUseCase> _mockUseCase;
        private readonly Mock<IGetAssetRegisterVersionsUseCase> _mockGetUseCase;

        [SetUp]
        public void  Setup()
        {
            _mockUseCase = new Mock<ISaveUploadedAssetRegisterFileUseCase>();
            _classUnderTest = new AssetRegisterVersionController(_mockGetUseCase.Object,_mockUseCase.Object);
        }

        [Test]
        public async Task GivenValidFile_WhenUploading_ThenCanSaveFile()
        {

        }
    }
}
