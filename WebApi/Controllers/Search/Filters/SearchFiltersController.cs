using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.UseCase.GetAssetDevelopers;
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
        private readonly IGetAssetDevelopersUseCase _assetDevelopersUseCase;

        public SearchFiltersController(IGetAssetRegionsUseCase assetRegionsUseCase, IGetAssetDevelopersUseCase assetDevelopersUseCase)
        {
            _assetRegionsUseCase = assetRegionsUseCase;
            _assetDevelopersUseCase = assetDevelopersUseCase;
        }

        [HttpGet("regions")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetRegionsResponse>), 200)]
        public async Task<IActionResult> GetRegions()
        {
            var response = await _assetRegionsUseCase.ExecuteAsync(this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<GetAssetRegionsResponse, AssetRegion>(response);
        }

        [HttpGet("developers")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetDevelopersResponse>), 200)]
        public async Task<IActionResult> GetDevelopers()
        {
            var response = await _assetDevelopersUseCase.ExecuteAsync(this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<GetAssetDevelopersResponse, AssetDeveloper>(response);
        }
    }
}
