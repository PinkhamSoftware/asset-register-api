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

        public async Task<AuthenticateUserResponse> ExecuteAsync(AuthenticateUserRequest request,
            CancellationToken cancellationToken)
        {
            if (!UserIsAuthorised(request.Email))
            {
                return UnauthorisedResponse();
            }

            var createdToken = await CreateAuthenticationTokenForEmail(request.Email, cancellationToken).ConfigureAwait(false);
            
            await SendOneTimeLink(createdToken, request.Url, cancellationToken).ConfigureAwait(false);

            return AuthorisedResponse();
        }

        private static AuthenticateUserResponse AuthorisedResponse()
        {
            return new AuthenticateUserResponse
            {
                Authorised = true
            };
        }

        private async Task SendOneTimeLink(IAuthenticationToken createdToken, string originUrl, CancellationToken cancellationToken)
        {
            await _oneTimeLinkNotifier.SendOneTimeLinkAsync(new OneTimeLinkNotification
            {
                Email = createdToken.ReferenceNumber,
                Token = createdToken.Token,
                Url = originUrl
            }, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IAuthenticationToken> CreateAuthenticationTokenForEmail(string email, CancellationToken cancellationToken)
        {
            IAuthenticationToken token = new AuthenticationToken
            {
                ReferenceNumber = email
            };

            IAuthenticationToken createdToken = await _authenticationTokenCreator.CreateAsync(token, cancellationToken).ConfigureAwait(false);
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
            var whitelist = System.Environment.GetEnvironmentVariable("EMAIL_WHITELIST").Split(";").ToList();
            return whitelist.Contains(email);
        }
    }
}
