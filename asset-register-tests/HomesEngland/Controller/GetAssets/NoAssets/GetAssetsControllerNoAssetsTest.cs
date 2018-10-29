using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using asset_register_api.Controllers;
using asset_register_api.Interface.UseCase;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace asset_register_tests.HomesEngland.Controller.GetAssets
{
    using AssetDictionary = Dictionary<string, string>;
    using AssetsDictionary = Dictionary<string, Dictionary<string, string>[]>;

    [TestFixture]
    public class GetAssetsControllerNoAssetsTest
    {

        private int[] AssetIds = {1, 2, 3, 4};
        private Mock<IGetAssetsUseCase> _mock;
        private AssetsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IGetAssetsUseCase>();
            _mock.Setup(useCase => useCase.Execute(AssetIds)).ReturnsAsync(
                () => new Dictionary<string, string>[0]);
            _controller = new AssetsController(_mock.Object);
        }

        [Test]
        public async Task ReturnsJsonWithEmptyAssetsArray()
        {
            ActionResult<AssetsDictionary> controllerResult = await  _controller.Get(AssetIds);
            Assert.AreEqual(
                controllerResult.Value,
                new AssetsDictionary {
                    {"Assets", new AssetDictionary[0]}
                }
            );
        }
    }
}
