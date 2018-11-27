﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.GenerateAssets;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using Main;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace AssetRegisterTests.HomesEngland.DataGenerator
{
    [TestFixture]
    public class ConsoleGeneratorAcceptanceTests
    {
        private readonly IConsoleGenerator _classUnderTest;
        private readonly ISearchAssetUseCase _searchAssetUseCase;

        public ConsoleGeneratorAcceptanceTests()
        {
            var assetRegister = new AssetRegister();
            var context = assetRegister.Get<AssetRegisterContext>();
            context.Database.Migrate();
            _classUnderTest = assetRegister.Get<IConsoleGenerator>();
            _searchAssetUseCase = assetRegister.Get<ISearchAssetUseCase>();
        }

        [TestCase("--records", "1")]
        [TestCase("--records", "2")]
        [TestCase("--records", "3")]
        public async Task GivenWeNeedToGenerateAssets_WhenWeDoSoThroughAConsoleInterface_ThenWeCanSearchForAssets(string arg1, string arg2)
        {
            //arrange
            var args = new string[] {arg1, arg2};
            //act
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await _classUnderTest.ProcessAsync(args).ConfigureAwait(false);
                //assert
                for (int i = 0; i < response.Count; i++)
                {
                    var generatedAsset = response.ElementAtOrDefault(i);

                    var record = await FindAsset(generatedAsset, i);
                    
                    record.Should().NotBeNull();
                    record.AssetOutputModelIsEqual(generatedAsset);
                }
                trans.Dispose();
            }
        }

        private async Task<AssetOutputModel> FindAsset(AssetOutputModel generatedAsset, int i)
        {
            var record = await _searchAssetUseCase.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = generatedAsset?.SchemeId
            }, CancellationToken.None).ConfigureAwait(false);
            return record.Assets.ElementAtOrDefault(0);
        }
    }
}
