using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.CalculateAssetAggregates.Models;

namespace HomesEngland.UseCase.CalculateAssetAggregates
{
    public class CalculateAssetAggregatesUseCase: ICalculateAssetAggregatesUseCase
    {
        private readonly IAssetAggregator _assetAggregator;

        public CalculateAssetAggregatesUseCase(IAssetAggregator assetAggregator)
        {
            _assetAggregator = assetAggregator;
        }

        public async Task<CalculateAssetAggregateResponse> ExecuteAsync(CalculateAssetAggregateRequest requests, CancellationToken cancellationToken)
        {
            var assetSearchQuery = new AssetSearchQuery
            {
                SchemeId = requests?.SchemeId,
                Address = requests?.Address,
                AssetRegisterVersionId = requests.AssetRegisterVersionId
            };
            var result = await _assetAggregator.Aggregate(assetSearchQuery, cancellationToken).ConfigureAwait(false);
            var response = new CalculateAssetAggregateResponse
            {
                AssetAggregates = new AssetAggregatesOutputModel(result)
            };
            return response;
        }
    }
}
