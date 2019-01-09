using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using HomesEngland.Gateway.Sql;
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

        public AssetSearchGatewayTests()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var assetGateway = new EFAssetGateway(databaseUrl);

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
                var createdAsset = await CreateAsset(schemeId, null, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);
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
                var createdAsset = await CreateAsset(schemeId, null, _gateway);
                var createdAsset2 = await CreateAsset(schemeId2, null, _gateway);
                var createdAsset3 = await CreateAsset(schemeId3, null, _gateway);

                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId2
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Count.Should().Be(1);
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset2.Id, createdAsset2);
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
                
                await _gateway.CreateAsync(entity).ConfigureAwait(false);
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.Should().BeNullOrEmpty();
                trans.Dispose();
            }
        }

        private async Task<IAsset> CreateAsset(int? schemeId, string address, IGateway<IAsset,int> gateway)
        {
            var entity = TestData.Domain.GenerateAsset();
            if(schemeId.HasValue)
                entity.SchemeId = schemeId;
            if (!string.IsNullOrEmpty(address))
                entity.Address = address;
            IAsset createdAsset = await gateway.CreateAsync(entity).ConfigureAwait(false);
            return createdAsset;
        }

        [TestCase("Address 1")]
        [TestCase("Address 2")]
        [TestCase("Address 3")]
        public async Task GivenAnAssetHasBeenCreated_WhenWeSearchViaExactAddress_ThenWeCanFindTheSameAsset(string address)
        {
            //arrange 
            using (var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdAsset = await CreateAsset(null, address, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = address
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);
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
                var createdAsset = await CreateAsset(null, address, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);
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
                var createdAsset = await CreateAsset(null, address, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);
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
                var createdAsset = await CreateAsset(null, address, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);
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
                await CreateAsset(null, address, _gateway);
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress
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
                await CreateAsset(null, null,_gateway);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
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
                var createdAsset = await CreateAsset(schemeId, address, _gateway);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
                };
                //act
                var assets = await _classUnderTest.Search(assetSearch, CancellationToken.None).ConfigureAwait(false);
                //assert
                assets.Results.ElementAtOrDefault(0).AssetIsEqual(createdAsset.Id, createdAsset);

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
                var createdAsset = await CreateAsset(schemeId, address, _gateway);
                var createdAsset2 = await CreateAsset(schemeId, address, _gateway);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    SchemeId = schemeId,
                    Address = searchAddress
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
                await CreateAsset(randomInt, address, _gateway);
                await CreateAsset(randomInt+1, address, _gateway);
                //act
                var assetSearch = new AssetPagedSearchQuery
                {
                    Address = searchAddress
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
                for (var i = 0; i < numberOfAssets; i++)
                {
                    
                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    await _gateway.CreateAsync(entity);
                }

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize
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
                for (var i = 0; i < numberOfAssets; i++)
                {
                    
                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    await _gateway.CreateAsync(entity);
                }

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize,
                    Page = page
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
                for (var i = 0; i < numberOfAssets; i++)
                {
                    
                    var entity = TestData.Domain.GenerateAsset();
                    entity.Address = address;
                    await _gateway.CreateAsync(entity);
                }

                var assetQuery = new AssetPagedSearchQuery
                {
                    Address = address,
                    PageSize = pageSize
                };

                var response = await _classUnderTest.Search(assetQuery, CancellationToken.None);

                response.NumberOfPages.Should().Be(expectedNumberOfPages);
                
                trans.Dispose();
            }
        }
    }
}
