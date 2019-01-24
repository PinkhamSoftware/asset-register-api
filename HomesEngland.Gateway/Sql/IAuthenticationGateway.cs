using HomesEngland.Gateway.AuthenticationTokens;

namespace HomesEngland.Gateway.Sql
{
    public interface IAuthenticationGateway : IOneTimeAuthenticationTokenCreator, IOneTimeAuthenticationTokenReader,
        IOneTimeAuthenticationTokenDeleter
    {
    }
}
