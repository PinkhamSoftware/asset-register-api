using System.Threading.Tasks;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Infrastructure.Api.Exceptions;
using Infrastructure.Api.Response.Validation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Search.Calculations
{
    [ApiVersion("1")]
    [Route("api/v{version:ApiVersion}/asset/search")]
    [ApiController]
    public class CalculateAssetAggregatesController : ControllerBase
    {
        private readonly ICalculateAssetAggregatesUseCase _useCase;
        public CalculateAssetAggregatesController(ICalculateAssetAggregatesUseCase useCase)
        {
            _useCase = useCase;
        }

        [MapToApiVersion("1")]
        [HttpGet("aggregation")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<CalculateAssetAggregateResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]CalculateAssetAggregateRequest request)
        {
            var result = await _useCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<CalculateAssetAggregateResponse, AssetAggregatesOutputModel>(result);
        }
    }
}
