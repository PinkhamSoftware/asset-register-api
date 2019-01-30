using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset;
using HomesEngland.UseCase.GetAsset.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Extensions.Requests;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IGetAssetUseCase _assetUseCase;

        public AssetController(IGetAssetUseCase useCase)
        {
            _assetUseCase = useCase;
        }

        [HttpGet("{id}")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<GetAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromRoute] GetAssetApiRequest request)
        {
            if (!request.IsValid())
            {
                return StatusCode(400);
            }

            GetAssetRequest getAssetRequest = new GetAssetRequest
            {
                Id = request.Id.Value
            };

            return this.StandardiseResponse<GetAssetResponse, AssetOutputModel>(
                await _assetUseCase.ExecuteAsync(getAssetRequest).ConfigureAwait(false));
        }
    }
}
