using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.Assets.Developer
{
    public interface IAssetDeveloperLister
    {
        Task<IList<AssetDeveloper>> ListDevelopersAsync(CancellationToken cancellationToken);
    }
}
