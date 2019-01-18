using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.UseCase.BulkCreateAsset.Models;

namespace HomesEngland.Gateway
{
    public interface IBulkAssetCreator
    {
        Task<IList<IAsset>> BulkCreateAsync(IAssetRegisterVersion assetRegisterVersion, CancellationToken cancellationToken);
    }
}
