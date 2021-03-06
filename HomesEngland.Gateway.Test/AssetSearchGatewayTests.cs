﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestHelper;

namespace HomesEngland.Gateway.Test
{
    [TestFixture]
    public class AssetSearchGatewayTests
    {
        private readonly IAssetSearcher _classUnderTest;
        private readonly IGateway<IAsset, int> _gateway;
        private readonly IAssetRegisterVersionCreator _assetRegisterVersionCreator;

        public AssetSearchGatewayTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);
            _assetRegisterVersionCreator = new EFAssetRegisterVersionGateway(databaseUrl);
            _gateway = assetGateway;

            _classUnderTest = assetGateway;

            var assetRegisterContext = new AssetRegisterContext(databaseUrl);
            assetRegisterContext.Database.Migrate();
        }

        [TestCase(5001)]
        [TestCase(5002)]
        [TestCase(5003)]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaSchemeIdThatHasBeenSet_ThenWeCanFindTheSameAsset(int schemeId)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(schemeId, null);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset})
                    .ConfigureAwait(false);

                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[0].Id, assetRegisterVersion.Assets[0]);
                trans.Dispose();
            }
        }


        [TestCase(7007, 7008, 7009)]
        [TestCase(2002, 2004, 2005)]
        [TestCase(3003, 3004, 3005)]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaSchemeIdThatHasBeenSet_ThenWeCanFindTheSameAssetAnd(int schemeId, int schemeId2, int schemeId3)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(schemeId, null);
                var createdAsset2 = CreateAsset(schemeId2, null);
                var createdAsset3 = CreateAsset(schemeId3, null);

                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> {createdAsset, createdAsset2, createdAsset3})
                    .ConfigureAwait(false);

                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId2,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Count.Should().Be(1);
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[1].Id, assetRegisterVersion.Assets[1]);
                trans.Dispose();
            }
        }

        [TestCase(4004)]
        [TestCase(5005)]
        [TestCase(6006)]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaSchemeIdThatHasntBeenSet_ThenWeCantFindTheSameAsset(int schemeId)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = TestData.Domain.GenerateAsset();

                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { entity })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Should().BeNullOrEmpty();
                trans.Dispose();
            }
        }

        private async Task<IAssetRegisterVersion> CreateAggregatedAssets(IList<IAsset> entities)
        {
            var assetRegisterVersion = await _assetRegisterVersionCreator.CreateAsync(new AssetRegisterVersion
            {
                Assets = entities,
                ModifiedDateTime = DateTime.UtcNow,

            }, CancellationToken.None);
            return assetRegisterVersion;
        }

        private IAsset CreateAsset(int? schemeId, string address, string region = null)
        {
            IAsset entity = TestData.Domain.GenerateAsset();
            if (schemeId.HasValue)
                entity.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                entity.Address = address;
            if (!string.IsNullOrEmpty(region)) 
                entity.ImsOldRegion = region;
            return entity;
        }

        private IAsset CreateAssetWithDeveloper(int? schemeId, string developer = null)
        {
            IAsset entity = TestData.Domain.GenerateAsset();
            if (schemeId.HasValue)
                entity.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(developer) && !string.IsNullOrWhiteSpace(developer))
                entity.DevelopingRslName = developer;
            return entity;
        }

        [TestCase("Address 1")]
        [TestCase("Address 2")]
        [TestCase("Address 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaExactAddress_ThenWeCanFindTheSameAsset(string address)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = address,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[0].Id, assetRegisterVersion.Assets[0]);
                trans.Dispose();
            }
        }

        [TestCase("Address 1", "Addr")]
        [TestCase("Address 2", "Addr")]
        [TestCase("Address 3", "Addr")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaStartsWith_ThenWeCanFindTheSameAsset(string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[0].Id, assetRegisterVersion.Assets[0]);
                trans.Dispose();
            }
        }

        [TestCase("Address 1", "ss 1")]
        [TestCase("Address 2", "ress 2")]
        [TestCase("Address 3", "3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaEndWith_ThenWeCanFindTheSameAsset(string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, address);
                var assetEntities = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress,
                    AssetRegisterVersionId = assetEntities.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetEntities.Assets[0].Id, assetEntities.Assets[0]);
                trans.Dispose();
            }
        }

        [TestCase("Address 1", "address 1")]
        [TestCase("Address 2", "addres")]
        [TestCase("Address 3", "Add")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaLowerCase_ThenWeCanFindTheSameAsset(string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[0].Id, assetRegisterVersion.Assets[0]);
                trans.Dispose();
            }
        }


        [TestCase(null, "address 1")]
        [TestCase(null, "addres")]
        [TestCase(null, "Add")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaForAnAddressThatDoesntExist_ThenWeReturnNullOrEmptyList(string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Should().BeNullOrEmpty();
                trans.Dispose();
            }
        }

        [TestCase(1111,null)]
        [TestCase(2222,null)]
        [TestCase(3333,null)]
        [TestCase(null,"add")]
        [TestCase(null,"Address 1")]
        [TestCase(null,"somewh")]
        [TestCase(null,"where")]
        [TestCase(null,"PO919")]
        [TestCase(null,"Tow111")]
        [TestCase(null,"3C03")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaFieldsThatHaventBeenSet_ThenWeGetNullOrEmptyArray(int? schemeId, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(null, null);

                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Should().BeNullOrEmpty();

                trans.Dispose();
            }
        }

        [TestCase(1111, null, null)]
        [TestCase(2222, null, null)]
        [TestCase(3333, null, null)]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "add")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "Address 1")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "somewh")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "where")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "PO57")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "Tow")]
        [TestCase(null, "Address 1, Somewhere road, Town, Region, PO57 C03", "C03")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "add")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "Address 3")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "somewh")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "where")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "PO57")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "Tow")]
        [TestCase(4567, "Address 3, Somewhere road, Town, Region, PO57 C03", "C03")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearch_ThenWeCanFindTheSameAsset(int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(schemeId, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(assetRegisterVersion.Assets[0].Id, assetRegisterVersion.Assets[0]);

                trans.Dispose();
            }
        }

        [TestCase(1111, null, null)]
        [TestCase(2222, null, null)]
        [TestCase(3333, null, null)]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "add")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "Address 8")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "somewh")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "where")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "PO57")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "Tow")]
        [TestCase(null, "Address 8, Somewhere road, Town, Region, PO57 C03", "C03")]
        public async Task GivenMultiplesAssetsHaveBeenCreatedWithASimilarAddress_WhenWeSearch_ThenWeCanFindMultipleAssets(int? schemeId, string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = CreateAsset(schemeId, address);
                var createdAsset2 = CreateAsset(schemeId, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset, createdAsset2 })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Count.Should().Be(2);

                trans.Dispose();
            }
        }


        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "add")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "Address 7")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "somewh")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "where")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "PO57")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "Tow")]
        [TestCase("Address 7, Somewhere road, Town, Region, PO57 C03", "C03")]
        public async Task GivenMultiplesAssetsHaveBeenCreatedWithASimilarAddress_WhenWeSearch_ThenTheAssetsAreOrderedBySchemeIdDesc(string address, string searchAddress)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var random = new Random();
                var randomInt = random.Next();
                var createdAsset = CreateAsset(randomInt, address);
                var createdAsset2 = CreateAsset(randomInt+1, address);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset,createdAsset2 })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                Assert.Greater(assets.Results.ElementAt(0).SchemeId, assets.Results.ElementAt(1).SchemeId);

                trans.Dispose();
            }
        }

        [TestCase("Meow",  1, 3, 1)]
        [TestCase("Woof",  2, 3, 2)]
        [TestCase("Moo",   3, 3, 3)]
        [TestCase("Cluck", 4, 3, 3)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfAssetsPerPage(string address, int pageSize, int numberOfAssets, int expectedNumberOfAssets)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = new List<IAsset>();
                for (var i = 0; i < numberOfAssets; i++)
                {
                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    assets.Add(entity);
                }

                var assetRegisterVersion = await CreateAggregatedAssets(assets)
                    .ConfigureAwait(false);

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.Results.Count.Should().Be(expectedNumberOfAssets);
                
                trans.Dispose();
            }
        }
        
        [TestCase("Meow",  1, 1, 3, 1)]
        [TestCase("Bark",  1, 2, 3, 1)]
        [TestCase("Woof",  2, 1, 3, 2)]
        [TestCase("Moo",   2, 2, 3, 1)]
        [TestCase("Quack", 4, 1, 3, 3)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfAssetsPerPage(string address, int pageSize, int page, int numberOfAssets, int expectedNumberOfAssets)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = new List<IAsset>();
                for (var i = 0; i < numberOfAssets; i++)
                {
                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    assets.Add(entity);
                }

                var assetRegisterVersion = await CreateAggregatedAssets(assets)
                    .ConfigureAwait(false);

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize,
                    Page = page,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.Results.Count.Should().Be(expectedNumberOfAssets);
                
                trans.Dispose();
            }
        }
        
        [TestCase("Meow", 1, 3, 3)]
        [TestCase("Woof", 2, 3, 2)]
        [TestCase("Moo", 3, 3, 1)]
        [TestCase("Cluck", 4, 3, 1)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchWithPageSize_ReturnCorrectNumberOfPages(string address, int pageSize, int numberOfAssets, int expectedNumberOfPages)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = new List<IAsset>();
                for (var i = 0; i < numberOfAssets; i++)
                {

                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    assets.Add(entity);
                }

                var assetRegisterVersion = await CreateAggregatedAssets(assets)
                    .ConfigureAwait(false);

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.NumberOfPages.Should().Be(expectedNumberOfPages);
                
                trans.Dispose();
            }
        }

        [TestCase("Meow", 1, 3, 3)]
        [TestCase("Woof", 2, 3, 2)]
        [TestCase("Moo", 3, 3, 1)]
        [TestCase("Cluck", 4, 3, 1)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchRegionWithPageSize_ReturnCorrectNumberOfPages(string region, int pageSize, int numberOfAssets, int expectedNumberOfPages)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = new List<IAsset>();
                for (var i = 0; i < numberOfAssets; i++)
                {

                    var entity = TestData.Domain.GenerateAsset();
                    entity.ImsOldRegion = region;
                    assets.Add(entity);
                }

                var assetRegisterVersion = await CreateAggregatedAssets(assets)
                    .ConfigureAwait(false);

                var assetQuery = new AssetPagedSearchQuery
                {
                    Region = region,
                    PageSize = pageSize,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.NumberOfPages.Should().Be(expectedNumberOfPages);

                trans.Dispose();
            }
        }

        [TestCase("Region 123", "re")]
        [TestCase("Region 123", "Reg")]
        [TestCase("Region 123", "Region 1")]
        [TestCase("Region 123", "egion 12")]
        [TestCase("Region 123", "egion")]
        public async Task GivenMultiplesAssetsHaveBeenCreatedWithTheSameRegion_WhenWeSearch_ThenTheAssetsAreOrderedBySchemeIdDesc(string region, string searchRegion)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var random = new Random();
                var randomInt = random.Next();
                var createdAsset = CreateAsset(randomInt, null, region);
                var createdAsset2 = CreateAsset(randomInt + 1, null, region);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset, createdAsset2 })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Region = searchRegion,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                Assert.Greater(assets.Results.ElementAt(0).SchemeId, assets.Results.ElementAt(1).SchemeId);

                trans.Dispose();
            }
        }

        [TestCase("Region 123", "re")]
        [TestCase("Region 123", "Reg")]
        [TestCase("Region 123", "Region 1")]
        [TestCase("Region 123", "egion 12")]
        public async Task GivenAnAssetHasBeenCreatedWithTheSameRegion_WhenWeSearch_ThenWeCanFindIt(string region, string searchRegion)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var random = new Random();
                var randomInt = random.Next();
                var createdAsset = CreateAsset(randomInt, null, region);
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Region = searchRegion,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Count.Should().Be(1);

                trans.Dispose();
            }
        }

        [TestCase("Developer Meow", 1, 3, 3)]
        [TestCase("Developer Woof", 2, 3, 2)]
        [TestCase("Developer Moo", 3, 3, 1)]
        [TestCase("Developer Cluck", 4, 3, 1)]
        public async Task GivenMultipleAssetsHaveBeenCreated_WhenWeSearchDeveloperWithPageSize_ReturnCorrectNumberOfPages(string developer, int pageSize, int numberOfAssets, int expectedNumberOfPages)
        {
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var assets = new List<IAsset>();
                for (var i = 0; i < numberOfAssets; i++)
                {
                    var entity = CreateAssetWithDeveloper(null, developer);
                    assets.Add(entity);
                }

                var assetRegisterVersion = await CreateAggregatedAssets(assets)
                    .ConfigureAwait(false);

                var assetQuery = new AssetPagedSearchQuery
                {
                    Developer = developer,
                    PageSize = pageSize,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.NumberOfPages.Should().Be(expectedNumberOfPages);

                trans.Dispose();
            }
        }

        [TestCase("Developer 123", "de")]
        [TestCase("Developer 123", "Dev")]
        [TestCase("Developer 123", "Develope")]
        [TestCase("Developer 123", "loper 12")]
        public async Task GivenMultiplesAssetsHaveBeenCreatedWithTheSameDeveloper_WhenWeSearch_ThenTheAssetsAreOrderedBySchemeIdDesc(string developer, string searchDeveloper)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var random = new Random();
                var randomInt = random.Next();
                var createdAsset = CreateAssetWithDeveloper(randomInt, developer);
                var createdAsset2 = CreateAssetWithDeveloper(randomInt + 1, developer);
                var createdAsset3 = CreateAssetWithDeveloper(randomInt + 2, $"Random {randomInt + 2}");
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset, createdAsset2, createdAsset3 })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Developer = searchDeveloper,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                Assert.Greater(assets.Results.ElementAt(0).SchemeId, assets.Results.ElementAt(1).SchemeId);

                trans.Dispose();
            }
        }

        [TestCase("Developer 123", "de")]
        [TestCase("Developer 123", "Dev")]
        [TestCase("Developer 123", "Develope")]
        [TestCase("Developer 123", "loper 12")]
        public async Task GivenAnAssetHasBeenCreatedWithTheSameDeveloper_WhenWeSearch_ThenWeCanFindIt(string developer, string searchDeveloper)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var random = new Random();
                var randomInt = random.Next();
                var createdAsset = CreateAssetWithDeveloper(randomInt, developer);
                var createdAsset2 = CreateAssetWithDeveloper(randomInt, $"Random : {randomInt}");
                var assetRegisterVersion = await CreateAggregatedAssets(new List<IAsset> { createdAsset, createdAsset2 })
                    .ConfigureAwait(false);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Developer = searchDeveloper,
                    AssetRegisterVersionId = assetRegisterVersion.Id
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Count.Should().Be(1);

                trans.Dispose();
            }
        }
    }
}
