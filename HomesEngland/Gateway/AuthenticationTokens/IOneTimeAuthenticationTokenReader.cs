using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.AuthenticationTokens
{
    public interface IOneTimeAuthenticationTokenReader
    {
        Task<IAuthenticationToken> ReadAsync(string token, CancellationToken cancellationToken);
    }
}
