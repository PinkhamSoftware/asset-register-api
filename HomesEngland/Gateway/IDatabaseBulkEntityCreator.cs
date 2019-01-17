using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway
{
    public interface IDatabaseBulkEntityCreator<T, TIndex> where T : IDatabaseEntity<TIndex>
    {
        Task<IList<T>> BulkCreateAsync(IList<T> entities, CancellationToken cancellationToken);
    }
}
