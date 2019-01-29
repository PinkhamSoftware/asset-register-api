using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;

namespace HomesEngland.Gateway.AssetRegisterVersions
{
    public interface IAssetRegisterVersionCreator
    {
        Task<IAssetRegisterVersion> CreateAsync(IAssetRegisterVersion assetRegisterVersion, CancellationToken cancellationToken);
    }
}
