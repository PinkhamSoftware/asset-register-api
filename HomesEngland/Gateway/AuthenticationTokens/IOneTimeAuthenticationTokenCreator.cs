using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.AuthenticationTokens
{
    public interface IOneTimeAuthenticationTokenCreator
    {
        Task<IAuthenticationToken> CreateAsync(IAuthenticationToken token, CancellationToken cancellationToken);
    }
}
