using System.Threading.Tasks;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Infrastructure.Api.Response;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search.Calculations
{
    
    [Route("api/v1/asset/search")]
    [ApiController]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 500)]
    public class CalculateAssetAggregatesController : ControllerBase
    {
        private readonly ICalculateAssetAggregatesUseCase _useCase;
        public CalculateAssetAggregatesController(ICalculateAssetAggregatesUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("aggregation")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ApiResponse<CalculateAssetAggregateResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]CalculateAssetAggregateRequest request)
        {
            var result = await _useCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<CalculateAssetAggregateResponse, AssetAggregatesOutputModel>(result);
        }
    }
}
