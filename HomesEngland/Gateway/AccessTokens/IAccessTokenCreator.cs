using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.AccessTokens
{
    public interface IAccessTokenCreator
    {
        Task<IAccessToken> CreateAsync(CancellationToken cancellationToken);
    }
}
