using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class AssetRegisterVersionSearcherTests
    {
        private IAssetRegisterVersionSearcher _classUnderTest;
        private IAssetRegisterVersionCreator _assetRegisterVersionCreator;

        [SetUp]
        public void Setup()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetRegisterVersionGateway(databaseUrl);
            _classUnderTest = assetGateway;
            _assetRegisterVersionCreator = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [Test]
        public async Task GivenMultiplesAssetsHaveBeenCreatedWithASimilarAddress_WhenWeSearch_ThenTheAssetsAreOrderedBySchemeIdDesc()
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await CreateAssetRegisterVersions(2);
                //act
                var query = new PagedQuery
                {
                    PageSize = 2,
                    Page = 1
                };
                //act
                var assets = await _classUnderTest.Search(query, CancellationToken.None).ConfigureAwait(false);
                //assert
                Assert.Greater(assets.Results.ElementAt(0).Id, assets.Results.ElementAt(1).Id);

                trans.Dispose();
            }
        }

        private async Task<IList<IAssetRegisterVersion>> CreateAssetRegisterVersions(int count)
        {
            IList<IAssetRegisterVersion> assetRegisterVersions = new List<IAssetRegisterVersion>();
            for (int i = 0; i < count; i++)
            {
                var assetRegisterVersion = new AssetRegisterVersion
                {
                    Assets = new List<IAsset>
                    {
                        new Asset
                        {
                            Address = $"test {i}"
                        }
                    }
                };
                assetRegisterVersions.Add(await _assetRegisterVersionCreator.CreateAsync(assetRegisterVersion, CancellationToken.None).ConfigureAwait(false));
            }

            return assetRegisterVersions;
        }

        [TestCase(1, 3, 1)]
        [TestCase(2, 3, 2)]
        [TestCase(3, 3, 3)]
        [TestCase(4, 3, 3)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfAssetsPerPage(int pageSize, int numberOfAssets, int expectedNumberOfAssets)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await CreateAssetRegisterVersions(numberOfAssets);

                var assetQuery = new PagedQuery
                {
                    PageSize = pageSize,
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.Results.Count.Should().Be(expectedNumberOfAssets);

                trans.Dispose();
            }
        }

        [TestCase("Meow", 1, 1, 3, 1)]
        [TestCase("Bark", 1, 2, 3, 1)]
        [TestCase("Woof", 2, 1, 3, 2)]
        [TestCase("Moo", 2, 2, 3, 1)]
        [TestCase("Quack", 4, 1, 3, 3)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfAssetsPerPage(string address, int pageSize, int page, int numberOfAssets, int expectedNumberOfAssets)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await CreateAssetRegisterVersions(numberOfAssets);

                var assetQuery = new PagedQuery
                {
                    PageSize = pageSize,
                    Page = page
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.Results.Count.Should().Be(expectedNumberOfAssets);

                trans.Dispose();
            }
        }

        [TestCase(1, 3, 3)]
        [TestCase(2, 3, 2)]
        [TestCase(3, 3, 1)]
        [TestCase(4, 3, 1)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfPages(int pageSize, int numberOfAssets, int expectedNumberOfPages)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var list = await CreateAssetRegisterVersions(numberOfAssets);

                var assetQuery = new PagedQuery
                {
                    PageSize = pageSize,
                    Page = 1,
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.NumberOfPages.Should().Be(expectedNumberOfPages);

                trans.Dispose();
            }
        }
    }
}
