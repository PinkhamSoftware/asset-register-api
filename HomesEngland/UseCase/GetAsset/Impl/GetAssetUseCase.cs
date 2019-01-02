using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Exception;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.GetAsset.Models;
using Infrastructure.Api.Exceptions;

namespace HomesEngland.UseCase.GetAsset.Impl
{
    public class GetAssetUseCase : IGetAssetUseCase
    {
        private readonly IAssetReader _assetReader;

        public GetAssetUseCase(IAssetReader assetReader)
        {
            _assetReader = assetReader;
        }

        public async Task<GetAssetResponse> ExecuteAsync(GetAssetRequest request)
        {
            IAsset asset = await _assetReader.ReadAsync(request.Id).ConfigureAwait(false);

            if (asset == null)
            {
                throw new AssetNotFoundException();
            }

            return new GetAssetResponse
            {
                Asset = new AssetOutputModel(asset)
            };
        }
    }
}
