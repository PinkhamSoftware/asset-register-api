using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using FluentAssertions;
using FluentSim;
using HomesEngland.UseCase.AuthenticateUser;
using HomesEngland.UseCase.AuthenticateUser.Models;
using HomesEngland.UseCase.GetAccessToken;
using HomesEngland.UseCase.GetAccessToken.Models;
using Main;
using NUnit.Framework;

namespace AssetRegisterTests.HomesEngland.UseCases
{
    [TestFixture]
    public class AuthenticationTests
    {
        private readonly IAuthenticateUser _authenticateUser;
        private readonly IGetAccessToken _getAccessToken;

        public AuthenticationTests()
        {
            var assetRegister = new AssetRegister();
            _authenticateUser = assetRegister.Get<IAuthenticateUser>();
        }

        private TransactionScope ATransaction()
        {
            return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        private class NotifyRequest
        {
            public string email_address { get; set; }
            public string template_id { get; set; }
            public NotifyPersonalisation personalisation { get; set; }
        }

        private class NotifyPersonalisation
        {
            public string access_url { get; set; }
        }

        private static string BuildValidGovNotifyApiKeyFromHexFragment(string fragment)
        {
            return
                $"{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}";
        }

        [Test]
        public async Task GivenUserIsAuthorised_SendAnEmailContainingATokenToTheUser()
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", "http://localhost:7654/");
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY", BuildValidGovNotifyApiKeyFromHexFragment("1111"));
            Environment.SetEnvironmentVariable("EMAIL_WHITELIST", "test@example.com");
            using (ATransaction())
            {
                var simulator = new FluentSimulator("http://localhost:7654/");
                simulator.Start();
                simulator.Post("/v2/notifications/email").Responds().WithCode(200);

                AuthenticateUserRequest request = new AuthenticateUserRequest
                {
                    Email = "test@example.com",
                    Url = "http://meow.cat/"
                };

                await _authenticateUser.ExecuteAsync(request, CancellationToken.None);

                var notifyRequest = simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();
                simulator.Stop();

                notifyRequest.Should().NotBeNull();
                notifyRequest.email_address.Should().Be("test@example.com");
                notifyRequest.personalisation.access_url.Should().Contain("http://meow.cat/");
            }
        }

        [Test]
        [Ignore("Ignored whilst token creator is not implemented")]
        public async Task GivenUserIsAuthorised_AndTheyGetAOneTimeUseToken_TheyCanGetAnApiKeyWithTheirToken()
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", "http://localhost:7654/");
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY", BuildValidGovNotifyApiKeyFromHexFragment("1111"));
            Environment.SetEnvironmentVariable("EMAIL_WHITELIST", "test@example.com");
            using (ATransaction())
            {
                var simulator = new FluentSimulator("http://localhost:7654/");
                simulator.Start();
                simulator.Post("/v2/notifications/email").Responds().WithCode(200);

                AuthenticateUserRequest request = new AuthenticateUserRequest
                {
                    Email = "test@example.com",
                    Url = "http://meow.cat/"
                };

                await _authenticateUser.ExecuteAsync(request, CancellationToken.None);

                NotifyRequest notifyRequest = simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();
                string token = GetTokenFromNotifyRequest(notifyRequest);

                GetAccessTokenRequest tokenRequest = new GetAccessTokenRequest
                {
                    Token = token
                };

                GetAccessTokenResponse response =
                    await _getAccessToken.ExecuteAsync(tokenRequest, CancellationToken.None);

                response.Should().NotBeNull();
                response.AccessToken.Should().NotBeNull();
            }
        }

        private string GetTokenFromNotifyRequest(NotifyRequest notifyRequest)
        {
            Uri accessUri = new Uri(notifyRequest.personalisation.access_url);

            return HttpUtility.ParseQueryString(accessUri.Query).Get("token");
        }
    }


}
