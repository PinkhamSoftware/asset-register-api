using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway
{
    public interface IDatabaseEntitySearcher<T, TIndex, TSearch> where T : IDatabaseEntity<TIndex>
    {
        Task<IPagedResults<T>> Search(TSearch searchRequest, CancellationToken cancellationToken);
    }
}
