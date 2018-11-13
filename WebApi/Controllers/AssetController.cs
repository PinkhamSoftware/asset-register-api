﻿using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using Infrastructure.Api.Response;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    public class AssetController : ControllerBase
    {
        private readonly IGetAssetUseCase _assetUseCase;
        public AssetController(IGetAssetUseCase useCase)
        {
            _assetUseCase = useCase;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<GetAssetResponse>), 200)]
        public async Task<IActionResult> Get(GetAssetRequest request)
        {
            var result = await _assetUseCase.ExecuteAsync(request).ConfigureAwait(false);
            return this.StandardiseResponse(result);
        }
    }
}
