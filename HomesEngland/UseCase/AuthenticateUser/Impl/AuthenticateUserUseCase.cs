using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.Notifications;
using HomesEngland.UseCase.AuthenticateUser.Models;

namespace HomesEngland.UseCase.AuthenticateUser.Impl
{
    public class AuthenticateUserUseCase : IAuthenticateUser
    {
        private readonly IOneTimeAuthenticationTokenCreator _authenticationTokenCreator;
        private readonly IOneTimeLinkNotifier _oneTimeLinkNotifier;

        public AuthenticateUserUseCase(IOneTimeAuthenticationTokenCreator authenticationTokenCreator,
            IOneTimeLinkNotifier notifier)
        {
            _authenticationTokenCreator = authenticationTokenCreator;
            _oneTimeLinkNotifier = notifier;
        }

        public async Task<AuthenticateUserResponse> ExecuteAsync(AuthenticateUserRequest requests,
            CancellationToken cancellationToken)
        {
            if (!UserIsAuthorised(requests.Email))
            {
                return UnauthorisedResponse();
            }

            var createdToken = await CreateAuthenticationTokenForEmail(cancellationToken).ConfigureAwait(false);
            
            await SendOneTimeLink(requests.Email, createdToken, requests.Url, cancellationToken).ConfigureAwait(false);

            return AuthorisedResponse();
        }

        private static AuthenticateUserResponse AuthorisedResponse()
        {
            return new AuthenticateUserResponse
            {
                Authorised = true
            };
        }

        private async Task SendOneTimeLink(string email, IAuthenticationToken createdToken, string originUrl, CancellationToken cancellationToken)
        {
            await _oneTimeLinkNotifier.SendOneTimeLinkAsync(new OneTimeLinkNotification
            {
                Email = email,
                Token = createdToken.Token,
                Url = originUrl
            }, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IAuthenticationToken> CreateAuthenticationTokenForEmail(CancellationToken cancellationToken)
        {
            var authenticationToken = new AuthenticationToken
            {
                Expiry = DateTime.UtcNow.AddHours(8),
                Token = Guid.NewGuid().ToString(),
                ReferenceNumber = Guid.NewGuid().ToString()
            };
            IAuthenticationToken createdToken = await _authenticationTokenCreator.CreateAsync(authenticationToken, cancellationToken).ConfigureAwait(false);
            return createdToken;
        }

        private AuthenticateUserResponse UnauthorisedResponse()
        {
            return new AuthenticateUserResponse
            {
                Authorised = false
            };
        }

        private bool UserIsAuthorised(string email)
        {
            var whitelist = System.Environment.GetEnvironmentVariable("EMAIL_WHITELIST").Split(";").Select(s=> s.ToLower()).ToList();
            return whitelist.Contains(email.ToLower());
        }
    }
}
