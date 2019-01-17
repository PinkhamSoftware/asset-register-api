using System.Threading;
using System.Threading.Tasks;

namespace HomesEngland.Gateway.AuthenticationTokens
{
    public interface IOneTimeAuthenticationTokenDeleter
    {
        Task DeleteAsync(string token, CancellationToken cancellationToken);
    }
}
