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
        private IOneTimeAuthenticationTokenCreator _authenticationTokenCreator;
        private IOneTimeLinkNotifier _oneTimeLinkNotifier;

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

            IAuthenticationToken token = new AuthenticationToken
            {
                Email = request.Email
            };

            IAuthenticationToken createdToken = await _authenticationTokenCreator.CreateAsync(token);

            await _oneTimeLinkNotifier.SendOneTimeLinkAsync(new OneTimeLinkNotification
            {
                Email = createdToken.Email,
                Token = createdToken.Token
            });

            return new AuthenticateUserResponse
            {
                Authorised = true
            };
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
