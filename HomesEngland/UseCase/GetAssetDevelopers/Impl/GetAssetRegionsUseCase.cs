using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Gateway.Assets.Developer;
using HomesEngland.UseCase.GetAssetRegions.Models;

namespace HomesEngland.UseCase.GetAssetDevelopers.Impl
{
    public class GetAssetDevelopersUseCase : IGetAssetDevelopersUseCase
    {
        private readonly IAssetDeveloperLister _assetDeveloperLister;

        public GetAssetDevelopersUseCase(IAssetDeveloperLister assetDeveloperLister)
        {
            _assetDeveloperLister = assetDeveloperLister;
        }

        public async Task<GetAssetDevelopersResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var developers = await _assetDeveloperLister.ListDevelopersAsync(cancellationToken).ConfigureAwait(false);
            var response = new GetAssetDevelopersResponse
            {
                Developers = developers
            };
            return response;
        }
    }
}
