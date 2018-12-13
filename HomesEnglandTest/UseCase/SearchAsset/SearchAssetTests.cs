﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Impl;
using HomesEngland.UseCase.SearchAsset.Models;
using Infrastructure.Api.Exceptions;
using NSubstitute;
using NUnit.Framework;
using TestHelper;

namespace HomesEnglandTest.UseCase.SearchAsset
{
    [TestFixture]
    public class SearchAssetTests
    {
        private readonly ISearchAssetUseCase _classUnderTest;
        private readonly IAssetSearcher _mockGateway;

        public SearchAssetTests()
        {
            _mockGateway = Substitute.For<IAssetSearcher>();
            _classUnderTest = new SearchAssetUseCase(_mockGateway);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GivenValidSchemeId_UseCaseCallsGatewayWithCorrectId(int id)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            asset.SchemeId = id;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});
            //act
            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = id
            }, CancellationToken.None);
            //assert
            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.SchemeId == id), Arg.Any<CancellationToken>());
        }

        [TestCase("address1")]
        [TestCase("address2")]
        [TestCase("address3")]
        public async Task GivenValidAddress_UseCaseCallsGatewayWithCorrectId(string address)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});
            //act
            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address
            }, CancellationToken.None);
            //assert
            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.Address == address), Arg.Any<CancellationToken>());
        }

        [TestCase("Address1")]
        [TestCase("Address2")]
        [TestCase("Address3")]
        public async Task GivenValidAddressAndNoPage_UseCaseCallsGatewayWithPageOne(string address)
        {
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});

            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address
            }, CancellationToken.None);

            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.Page == 1), Arg.Any<CancellationToken>());
        }

        [TestCase("Address1", 1)]
        [TestCase("Address2", 2)]
        [TestCase("Address3", 3)]
        public async Task GivenValidAddressAndPageNumber_UseCaseCallsGatewayWithCorrectPage(string address, int page)
        {
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});

            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address,
                Page = page
            }, CancellationToken.None);

            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.Page == page), Arg.Any<CancellationToken>());
        }

        [TestCase("Address1")]
        [TestCase("Address2")]
        [TestCase("Address3")]
        public async Task GivenValidAddressAndNoPageSize_UseCaseCallsGatewayWithDefaultPageSize(string address)
        {
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});

            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address
            }, CancellationToken.None);

            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.PageSize == 25), Arg.Any<CancellationToken>());
        }

        [TestCase("Address1", 10)]
        [TestCase("Address2", 20)]
        [TestCase("Address3", 30)]
        public async Task GivenValidAddressAndPageSize_UseCaseCallsGatewayWithPageSize(string address, int pageSize)
        {
            var asset = TestData.Domain.GenerateAsset();
            asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None).Returns(new PagedResults<IAsset>
                {Results = new List<IAsset> {asset}});

            await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address,
                PageSize = pageSize
            }, CancellationToken.None);

            await _mockGateway.Received()
                .Search(Arg.Is<AssetPagedSearchQuery>(req => req.PageSize == pageSize), Arg.Any<CancellationToken>());
        }

        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(null, "d")]
        [TestCase(null, "e")]
        [TestCase(null, "t")]
        public async Task GivenValidRequestId_UseCaseReturnsCorrectlyMappedAsset(int? id, string address)
        {
            //arrange
            var asset = TestData.Domain.GenerateAsset();
            if (id.HasValue)
                asset.SchemeId = id;
            if (!string.IsNullOrEmpty(address))
                asset.Address = address;
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {asset}});
            //act
            var response = await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = id,
                Address = address
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Assets.Should().NotBeNullOrEmpty();
            response.Assets[0].Should().BeEquivalentTo(asset);
        }

        [TestCase(1, 2)]
        [TestCase(3, 4)]
        [TestCase(5, 6)]
        public async Task GivenValidRequestIdAndMultipleAssetsReturned_UseCaseReturnsCorrectlyMappedAssets(int idOne,
            int idTwo)
        {
            var assetOne = TestData.Domain.GenerateAsset();
            assetOne.SchemeId = idOne;

            var assetTwo = TestData.Domain.GenerateAsset();
            assetOne.SchemeId = idTwo;

            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {Results = new List<IAsset> {assetOne, assetTwo}});
            //act
            var response = await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                SchemeId = idOne
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Assets.Should().NotBeNullOrEmpty();
            response.Assets[0].Should().BeEquivalentTo(assetOne);
            response.Assets[1].Should().BeEquivalentTo(assetTwo);
        }

        [TestCase("address1", 1)]
        [TestCase("address2", 2)]
        [TestCase("address3", 3)]
        public async Task GivenValidRequestAndNumberOfPagesReturned_UseCaseReturnsCorrectPageInformation(
            string address, int numberOfPages)
        {
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {NumberOfPages = numberOfPages});

            var response = await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address
            }, CancellationToken.None);

            response.Should().NotBeNull();
            response.Pages.Should().Be(numberOfPages);
        }

        [TestCase("address1", 1)]
        [TestCase("address2", 2)]
        [TestCase("address3", 3)]
        public async Task GivenValidRequestAndTotalCount_UseCaseReturnsCorrectTotalCount(string address,
            int totalNumberFound)
        {
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset> {TotalCount = totalNumberFound});

            var response = await _classUnderTest.ExecuteAsync(new SearchAssetRequest
            {
                Address = address
            }, CancellationToken.None);

            response.Should().NotBeNull();
            response.TotalCount.Should().Be(totalNumberFound);
        }

        [TestCase(1, null)]
        [TestCase(2, "")]
        [TestCase(3, " ")]
        [TestCase(null, "a")]
        [TestCase(null, "b")]
        [TestCase(null, "c")]
        public async Task GivenValidInput_WhenNoAssetsFoundForSchemeId_ThenUseCaseThrowsAssetReturnsNull(int? id,
            string address)
        {
            //arrange
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns(new PagedResults<IAsset>());
            //act
            var response = await _classUnderTest.ExecuteAsync(new SearchAssetRequest {SchemeId = id, Address = address},
                CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Assets.Should().BeNullOrEmpty();
        }

        [TestCase(1, null)]
        [TestCase(2, null)]
        [TestCase(3, null)]
        [TestCase(null, "d")]
        [TestCase(null, "e")]
        [TestCase(null, "t")]
        public async Task GivenValidRequest_WhenSearchResultsAreNull_ThenUseCaseThrowsAssetNotFoundException(int id,
            string address)
        {
            //arrange
            _mockGateway.Search(Arg.Any<IAssetPagedSearchQuery>(), CancellationToken.None)
                .Returns((IPagedResults<IAsset>) null);
            var searchAssetRequest = new SearchAssetRequest
            {
                SchemeId = id,
                Address = address
            };
            //act
            //act
            var response = await _classUnderTest.ExecuteAsync(searchAssetRequest, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Assets.Should().BeNullOrEmpty();
        }

        [TestCase(null, null)]
        [TestCase(0, null)]
        [TestCase(-1, null)]
        [TestCase(null, "")]
        [TestCase(null, " ")]
        public void GivenInValidRequest_ThenUseCaseThrowsBadRequestException(int? id, string address)
        {
            //arrange
            IAssetPagedSearchQuery pagedSearchQueryAssetPagedRequest = new AssetPagedSearchQuery
            {
                SchemeId = id,
                Address = address
            };
            var assetRequest = new SearchAssetRequest {SchemeId = id};
            _mockGateway.Search(pagedSearchQueryAssetPagedRequest, CancellationToken.None).Returns((IPagedResults<IAsset>) null);
            //act
            //assert
            Assert.ThrowsAsync<BadRequestException>(async () =>
                await _classUnderTest.ExecuteAsync(assetRequest, CancellationToken.None));
        }

        [Test]
        public void GivenAnInvalidRequest_ThenWeThrowBadRequestException()
        {
            //arrange 
            SearchAssetRequest assetSearch = null;
            //act 
            //assert
            Assert.ThrowsAsync<BadRequestException>(async () =>
                await _classUnderTest.ExecuteAsync(assetSearch, CancellationToken.None).ConfigureAwait(false));
        }
    }
}
