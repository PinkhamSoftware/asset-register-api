using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search
{
    [Route("api/v1/asset/search/filters")]
    [ApiController]
    public class SearchFiltersController : ControllerBase
    {
        private readonly IAssetRegionLister _assetRegionLister;

        public SearchFiltersController(IAssetRegionLister assetRegionLister)
        {
            _assetRegionLister = assetRegionLister;
        }

        [HttpGet("regions")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<List<AssetRegion>>), 200)]
        public async Task<IActionResult> Get()
        {
            var result = await _assetRegionLister.ListRegionsAsync(this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<CalculateAssetAggregateResponse, AssetAggregatesOutputModel>(result);
        }
    }

}
