using System.Collections;
using HomesEngland.Domain;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;

namespace HomesEngland.Gateway.AssetRegisterVersions
{
    public interface IAssetRegisterVersionSearcher: IDatabaseEntitySearcher<IAssetRegisterVersion, int, IPagedQuery>
    {
        
    }
}
