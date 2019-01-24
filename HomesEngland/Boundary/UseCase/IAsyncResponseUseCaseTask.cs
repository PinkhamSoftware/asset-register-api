using System.Threading;
using System.Threading.Tasks;

namespace HomesEngland.Boundary.UseCase
{
    public interface IAsyncResponseUseCaseTask< TResponse>
    {
        Task<TResponse> ExecuteAsync(CancellationToken cancellationToken);
    }
}