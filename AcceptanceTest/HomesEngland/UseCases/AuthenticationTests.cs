using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using FluentSim;
using HomesEngland.UseCase.AuthenticateUser;
using HomesEngland.UseCase.AuthenticateUser.Models;
using Main;
using NUnit.Framework;

namespace AssetRegisterTests.HomesEngland.UseCases
{
    [TestFixture]
    public class AuthenticationTests
    {
        private readonly IAuthenticateUser _authenticateUser;

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

        }

        [Test]
        [Ignore("Notification gateway not yet implemented")]
        public async Task GivenUserIsAuthorised_SendAnEmailContainingATokenToTheUser()
        {
            System.Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", "http://meow.cat:7654/");
            System.Environment.SetEnvironmentVariable("EMAIL_WHITELIST", "test@example.com");
            using (ATransaction())
            {
                var simulator = new FluentSimulator("http://meow.cat:7654/");
                simulator.Post("/v2/notifications/email").Responds().WithCode(200);

                AuthenticateUserRequest request = new AuthenticateUserRequest
                {
                    Email = "test@example.com"
                };

                await _authenticateUser.ExecuteAsync(request, CancellationToken.None);

                var notifyRequest = simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

                notifyRequest.Should().NotBeNull();
            }
        }
    }
}
