using System.Threading;
using System.Threading.Tasks;

namespace HomesEngland.Boundary.UseCase
{
    public interface IAsyncUseCaseTask<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest requests, CancellationToken cancellationToken);
    }
}
