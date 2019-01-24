using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.CreateAsset.Models;

namespace HomesEngland.UseCase.CreateAsset
{
    public interface ICreateAssetUseCase : IAsyncUseCaseTask<CreateAssetRequest, CreateAssetResponse>
    {
    }
}
