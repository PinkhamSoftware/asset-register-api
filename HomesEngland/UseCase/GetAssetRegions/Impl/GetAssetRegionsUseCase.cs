using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Gateway.Assets.Region;
using HomesEngland.UseCase.GetAssetRegions.Models;

namespace HomesEngland.UseCase.GetAssetRegions.Impl
{
    public class GetAssetRegionsUseCase : IGetAssetRegionsUseCase
    {
        private readonly IAssetRegionLister _assetRegionLister;

        public GetAssetRegionsUseCase(IAssetRegionLister assetRegionLister)
        {
            _assetRegionLister = assetRegionLister;
        }

        public async Task<GetAssetRegionsResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var regions = await _assetRegionLister.ListRegionsAsync(cancellationToken).ConfigureAwait(false);
            var response = new GetAssetRegionsResponse
            {
                Regions = regions
            };
            return response;
        }
    }
}
