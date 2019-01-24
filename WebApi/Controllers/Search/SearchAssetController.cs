using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Extensions.Requests;

namespace WebApi.Controllers.Search
{
    [Route("api/v1/asset")]
    [ApiController]
    public class SearchAssetController : ControllerBase
    {
        private readonly ISearchAssetUseCase _useCase;
        private readonly IAssetRegisterVersionSearcher _assetRegisterVersionSearcher;

        public SearchAssetController(ISearchAssetUseCase useCase, IAssetRegisterVersionSearcher assetRegisterVersionSearcher)
        {
            _useCase = useCase;
            _assetRegisterVersionSearcher = assetRegisterVersionSearcher;
        }

        [HttpGet("search")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<SearchAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery] SearchAssetApiRequest request)
        {
            if (!request.IsValid())
            {
                return StatusCode(400);
            }

            var assetRegisterVersionId = await GetLatestAssetRegisterVersionIdIfNull(request);

            var searchAssetUseCaseRequest = new SearchAssetRequest
            {
                SchemeId = request.SchemeId,
                Address = request.Address,
                Page = request.Page,
                PageSize = request.PageSize,
                AssetRegisterVersionId = assetRegisterVersionId,
            };

            SearchAssetResponse result = await _useCase
                .ExecuteAsync(searchAssetUseCaseRequest, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<SearchAssetResponse, AssetOutputModel>(result);
        }

        private async Task<int?> GetLatestAssetRegisterVersionIdIfNull(SearchAssetApiRequest request)
        {
            int? assetRegisterVersionId = null;
            if (request.AssetRegisterVersionId.HasValue)
            {
                assetRegisterVersionId = request.AssetRegisterVersionId;
            }
            else
            {
                var latestAssetRegister = await _assetRegisterVersionSearcher.Search(new PagedQuery
                {
                    Page = 1,
                    PageSize = 1
                }, CancellationToken.None).ConfigureAwait(false);
                assetRegisterVersionId = latestAssetRegister?.Results?.ElementAtOrDefault(0)?.Id;
            }

            return assetRegisterVersionId;
        }
    }
}
