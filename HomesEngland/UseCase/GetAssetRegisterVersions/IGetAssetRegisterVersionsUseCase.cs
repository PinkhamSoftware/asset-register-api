using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;
using HomesEngland.UseCase.SearchAsset.Models;

namespace HomesEngland.UseCase.GetAssetRegisterVersions
{
    public interface IGetAssetRegisterVersionsUseCase : 
        IAsyncUseCaseTask<GetAssetRegisterVersionsRequest,GetAssetRegisterVersionsResponse>
    {

    }
}
