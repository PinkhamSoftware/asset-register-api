using System.Threading.Tasks;
using HomesEngland.Exception;
using HomesEngland.Gateway.Assets;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.GetAsset.Models.Validation;
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
            if (!IsValidRequest(request))
            {
                throw new BadRequestException();
            }

            var asset = await _assetReader.ReadAsync(request.Id.Value).ConfigureAwait(false);

            if (asset == null)
                throw new AssetNotFoundException();

            var response = new GetAssetResponse
            {
                Asset = new AssetOutputModel(asset)
            };
            return response;
        }

        private bool IsValidRequest(GetAssetRequest request)
        {
            if (request == null)
            {
                return false;
            }

            var validator = new GetAssetRequestValidator();
            var getAssetRequest = request;
            var validationResult = validator.Validate(getAssetRequest);
            return validationResult.IsValid;
        }
    }
}
