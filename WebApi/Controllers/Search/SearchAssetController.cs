using System.Threading.Tasks;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.SearchAsset;
using HomesEngland.UseCase.SearchAsset.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search
{
    [Route("api/v1/asset")]
    [ApiController]
    public class SearchAssetController : ControllerBase
    {
        private readonly ISearchAssetUseCase _useCase;

        public SearchAssetController(ISearchAssetUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("search")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<SearchAssetResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery] SearchAssetRequest request)
        {
            var result = await _useCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<SearchAssetResponse, AssetOutputModel>(result);
        }
    }
}
