using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Exception;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace HomesEngland.UseCase.CreateAsset.Impl
{
    public class CreateAssetUseCase : ICreateAssetUseCase
    {
        private readonly IAssetCreator _assetCreator;

        public CreateAssetUseCase(IAssetCreator assetCreator)
        {
            _assetCreator = assetCreator;
        }

        public async Task<CreateAssetResponse> ExecuteAsync(CreateAssetRequest requests, CancellationToken cancellationToken)
        {
            IAsset asset = new Asset(requests);

            var createdAsset = await _assetCreator.CreateAsync(asset);
            if(createdAsset == null)
                throw new CreateAssetException();
            
            var assetOutputModel = new AssetOutputModel(createdAsset);
            var createdAssetResponse = new CreateAssetResponse
            {
                Asset = assetOutputModel
            };
            return createdAssetResponse;
        }
    }
}
