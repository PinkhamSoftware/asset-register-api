using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AuthenticationTokens;

namespace HomesEngland.Gateway.Sql
{
    public class DummySqlAuthenticationTokenGateway : IOneTimeAuthenticationTokenCreator
    {
        public async Task<IAuthenticationToken> CreateAsync(IAuthenticationToken token)
        {
            return new AuthenticationToken
            {
                Email = "test@test.com",
                Token = "dummy",
            };
        }


    }
}
