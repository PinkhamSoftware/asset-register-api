using System.Threading.Tasks;
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

        public SearchAssetController(ISearchAssetUseCase useCase)
        {
            _useCase = useCase;
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

            SearchAssetRequest searchAssetUseCaseRequest = new SearchAssetRequest
            {
                SchemeId = request.SchemeId,
                Address = request.Address,
                Page = request.Page,
                PageSize = request.PageSize
            };

            SearchAssetResponse result = await _useCase
                .ExecuteAsync(searchAssetUseCaseRequest, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<SearchAssetResponse, AssetOutputModel>(result);
        }
    }
}
