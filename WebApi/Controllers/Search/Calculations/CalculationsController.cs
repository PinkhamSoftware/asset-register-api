using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.CalculateAssetAggregates;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Extensions.Requests;

namespace WebApi.Controllers.Search.Calculations
{
    
    [Route("api/v1/asset/search")]
    [ApiController]
    public class CalculateAssetAggregatesController : ControllerBase
    {
        private readonly ICalculateAssetAggregatesUseCase _useCase;
        private readonly IAssetRegisterVersionSearcher _assetRegisterVersionSearcher;

        public CalculateAssetAggregatesController(ICalculateAssetAggregatesUseCase useCase, IAssetRegisterVersionSearcher assetRegisterVersionSearcher)
        {
            _useCase = useCase;
            _assetRegisterVersionSearcher = assetRegisterVersionSearcher;
        }

        [HttpGet("aggregation")]
        [Produces("application/json", "text/csv")]
        [ProducesResponseType(typeof(ResponseData<CalculateAssetAggregateResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]CalculateAssetAggregateRequest request)
        {
            request.AssetRegisterVersionId = await GetLatestAssetRegisterVersionIdIfNull(request).ConfigureAwait(false);

            var result = await _useCase.ExecuteAsync(request, this.GetCancellationToken()).ConfigureAwait(false);
            return this.StandardiseResponse<CalculateAssetAggregateResponse, AssetAggregatesOutputModel>(result);
        }

        private async Task<int?> GetLatestAssetRegisterVersionIdIfNull(CalculateAssetAggregateRequest request)
        {
            int? assetRegisterVersionId = null;
            if (request.AssetRegisterVersionId.HasValue)
                assetRegisterVersionId = request.AssetRegisterVersionId;
            else
            {
                var latestAssetRegister = await _assetRegisterVersionSearcher.Search(new PagedQuery
                {
                    Page = 1,
                    PageSize = 1
                }, CancellationToken.None).ConfigureAwait(false);
                assetRegisterVersionId = latestAssetRegister?.Results?.ElementAtOrDefault(0)?.Id;
            }

            return assetRegisterVersionId;
        }
    }
}
