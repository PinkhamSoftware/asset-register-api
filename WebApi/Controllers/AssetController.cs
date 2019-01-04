using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using Infrastructure.Api.Response;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
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
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ApiResponse<GetAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromRoute]GetAssetRequest request)
        {
            var result = await _assetUseCase.ExecuteAsync(request).ConfigureAwait(false);
            return this.StandardiseResponse<GetAssetResponse, AssetOutputModel>(result);
        }
    }
}
