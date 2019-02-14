using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using HomesEngland.UseCase.GetAssetRegions;
using HomesEngland.UseCase.GetAssetRegions.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search
{
    [Route("api/v1/asset/search/filters")]
    [ApiController]
    public class SearchFiltersController : ControllerBase
    {
        private readonly IGetAssetRegionsUseCase _assetRegionUseCaseLister;


        public SearchFiltersController(IGetAssetRegionsUseCase assetRegionUseCaseLister)
        {
            _assetRegionUseCaseLister = assetRegionUseCaseLister;
        }

        [HttpGet("regions")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetRegionsResponse>), 200)]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }
    }

}
