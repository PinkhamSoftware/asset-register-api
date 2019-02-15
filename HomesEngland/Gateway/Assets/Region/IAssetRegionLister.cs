using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.Assets.Region
{
    public interface IAssetRegionLister
    {
        Task<IList<AssetRegion>> ListRegionsAsync(CancellationToken cancellationToken);
    }
}
