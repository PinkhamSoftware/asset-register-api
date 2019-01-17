using System;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.UseCase.GetAccessToken.Models;

namespace HomesEngland.UseCase.GetAccessToken.Impl
{
    public class GetAccessTokenUseCase : IGetAccessToken
    {
        private readonly IOneTimeAuthenticationTokenReader _tokenReader;
        private readonly IAccessTokenCreator _accessTokenCreator;

        public GetAccessTokenUseCase(IOneTimeAuthenticationTokenReader tokenReader,
            IAccessTokenCreator accessTokenCreator)
        {
            _tokenReader = tokenReader;
            _accessTokenCreator = accessTokenCreator;
        }

        public async Task<GetAccessTokenResponse> ExecuteAsync(GetAccessTokenRequest tokenRequest,
            CancellationToken cancellationToken)
        {
            IAuthenticationToken token = await _tokenReader.ReadAsync(tokenRequest.Token, cancellationToken);

            if (NoMatchingTokenIsFound(token) || TokenHasExpired(token))
            {
                return UnauthorisedResponse();
            }

            IAccessToken accessToken = await _accessTokenCreator.CreateAsync(cancellationToken);

            return AuthorisedResponse(accessToken);
        }

        private static GetAccessTokenResponse AuthorisedResponse(IAccessToken accessToken)
        {
            return new GetAccessTokenResponse
            {
                Authorised = true, AccessToken = accessToken.Token
            };
        }

        private static bool TokenHasExpired(IAuthenticationToken token)
        {
            return token.Expiry < DateTime.Now;
        }

        private static GetAccessTokenResponse UnauthorisedResponse()
        {
            return new GetAccessTokenResponse {Authorised = false};
        }

        private static bool NoMatchingTokenIsFound(IAuthenticationToken token)
        {
            return token == null;
        }
    }
}
