using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Gateway.Assets.Developer;
using HomesEngland.UseCase.GetAssetRegions.Models;

namespace HomesEngland.UseCase.GetAssetDevelopers.Impl
{
    public class GetAssetDevelopersUseCase : IGetAssetDevelopersUseCase
    {
        private readonly IAssetDeveloperLister _assetRegionLister;

        public GetAssetDevelopersUseCase(IAssetDeveloperLister assetRegionLister)
        {
            _assetRegionLister = assetRegionLister;
        }

        public async Task<GetAssetDevelopersResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var developers = await _assetRegionLister.ListDevelopersAsync(cancellationToken).ConfigureAwait(false);
            var response = new GetAssetDevelopersResponse
            {
                Developers = developers
            };
            return response;
        }
    }
}
