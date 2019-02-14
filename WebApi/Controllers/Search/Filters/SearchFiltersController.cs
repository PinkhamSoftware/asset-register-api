using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.GetAssetRegions.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search.Filters
{
    [Route("api/v1/asset/search/filters")]
    [ApiController]
    public class SearchFiltersController : ControllerBase
    {
        private readonly IGetAssetRegionsUseCase _assetRegionsUseCase;

        public SearchFiltersController(IGetAssetRegionsUseCase assetRegionsUseCase)
        {
            _assetRegionsUseCase = assetRegionsUseCase;
        }

        [HttpGet("regions")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetRegionsResponse>), 200)]
        public async Task<IActionResult> Get()
        {
            var response = await _assetRegionsUseCase.ExecuteAsync(this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<GetAssetRegionsResponse, AssetRegion>(response);
        }
    }
}
