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

            var createdToken = await CreateAuthenticationTokenForEmail(request.Email);

            await SendOneTimeLink(createdToken, request.Url);

            return AuthorisedResponse();
        }

        private static AuthenticateUserResponse AuthorisedResponse()
        {
            return new AuthenticateUserResponse
            {
                Authorised = true
            };
        }

        private async Task SendOneTimeLink(IAuthenticationToken createdToken, string originUrl)
        {
            await _oneTimeLinkNotifier.SendOneTimeLinkAsync(new OneTimeLinkNotification
            {
                Email = createdToken.Email,
                Token = createdToken.Token,
                Url = originUrl
            });
        }

        private async Task<IAuthenticationToken> CreateAuthenticationTokenForEmail(string email)
        {
            IAuthenticationToken token = new AuthenticationToken
            {
                Email = email
            };

            IAuthenticationToken createdToken = await _authenticationTokenCreator.CreateAsync(token);
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
