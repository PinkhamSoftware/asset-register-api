using System.Collections.Generic;
using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.CreateAsset.Models;

namespace HomesEngland.UseCase.CreateAssetRegisterVersion
{
    public interface ICreateAssetRegisterVersionUseCase : IAsyncUseCaseTask<IList<CreateAssetRequest>, IList<CreateAssetResponse>>
    {
    }
}
