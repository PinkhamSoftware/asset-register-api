using HomesEngland.Domain;
using HomesEngland.UseCase.BulkCreateAsset.Models;

namespace HomesEngland.Gateway.AssetRegisterVersions
{
    public interface IAssetRegisterVersionSearcher: IDatabaseEntitySearcher<IAssetRegisterVersion, int, IPagedQuery>
    {
        
    }
}
