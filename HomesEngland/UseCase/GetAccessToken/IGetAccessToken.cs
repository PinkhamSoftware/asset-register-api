using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAccessToken.Models;

namespace HomesEngland.UseCase.GetAccessToken
{
    public interface IGetAccessToken
    {
        Task<GetAccessTokenResponse> ExecuteAsync(GetAccessTokenRequest tokenRequest, CancellationToken none);
    }
}
