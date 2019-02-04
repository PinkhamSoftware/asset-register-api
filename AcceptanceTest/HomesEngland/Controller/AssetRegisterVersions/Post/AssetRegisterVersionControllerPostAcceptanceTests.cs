﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.UseCase.GetAssetRegisterVersions;
using HomesEngland.UseCase.ImportAssets;
using HomesEngland.UseCase.ImportAssets.Models;
using HomesEngland.UseCase.SaveFile;
using Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApi.BackgroundProcessing;
using WebApi.Controllers;
using WebApi.Extensions;

namespace AssetRegisterTests.HomesEngland.Controller.AssetRegisterVersions.Post
{
    [TestFixture]
    public class AssetRegisterVersionControllerPostAcceptanceTests
    {
        private AssetRegisterVersionController _classUnderTest;

        [SetUp]
        public void Setup()
        {
            var assetRegister = new AssetRegister();
            var importUseCase = assetRegister.Get<IImportAssetsUseCase>();
            var textSplitter = assetRegister.Get<ITextSplitter>();
            var getAssetRegisterVersionUseCase = assetRegister.Get<IGetAssetRegisterVersionsUseCase>();
            var saveAssetRegisterFileUseCase = assetRegister.Get<ISaveAssetRegisterFileUseCase>();
            var backgroundProcessor = assetRegister.Get<IBackgroundProcessor>();
            _classUnderTest = new AssetRegisterVersionController(getAssetRegisterVersionUseCase, importUseCase,textSplitter, backgroundProcessor);
        }

        [TestCase(1, "asset-register-1-rows.csv")]
        [TestCase(5, "asset-register-5-rows.csv")]
        [TestCase(10, "asset-register-10-rows.csv")]
        public async Task GivenValidFile_WhenUploading_ThenCanImport(int expectedCount,string fileValue)
        {
            //arrange
            var formFiles = await GetFormFiles(fileValue);
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.Post(formFiles);
                //asset
                var result = response as ObjectResult;
                result.Should().NotBeNull();
                result.Value.Should().BeOfType<ResponseData<ImportAssetsResponse>>();
                var data = result.Value as ResponseData<ImportAssetsResponse>;
                data.Data.AssetsImported.Count.Should().Be(expectedCount);
            }
        }

        private async Task<List<IFormFile>> GetFormFiles(string fileValue)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory,"HomesEngland", "Controller", "AssetRegisterVersions", "Post", fileValue);
            var fileStream = await File.ReadAllBytesAsync(path).ConfigureAwait(false);
            var memoryStream = new MemoryStream(fileStream);
            var formFiles = new List<IFormFile>
                {new FormFile(memoryStream, 0, memoryStream.Length, fileValue, fileValue)};
            return formFiles;
        }
    }
}
