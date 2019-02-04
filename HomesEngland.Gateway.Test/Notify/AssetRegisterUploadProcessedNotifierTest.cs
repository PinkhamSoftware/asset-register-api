using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSim;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.Notifications;
using HomesEngland.Gateway.Notify;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test.Notify
{
    public class AssetRegisterUploadProcessedNotifierTest
    {
        private FluentSimulator _simulator;
        private string _govNotifyUrl;
        private string _govNotifyApiKey;
        private IAssetRegisterUploadProcessedNotifier _classUnderTest;

        private class NotifyRequest
        {
            public string email_address { get; set; }
        }

        private static string BuildValidGovNotifyApiKeyFromHexFragment(string fragment)
        {
            return
                $"{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}";
        }

        private static void SetupEnvironmentVariables(string notifyUrl, string apiKeyFragment)
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", notifyUrl);
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY",
                BuildValidGovNotifyApiKeyFromHexFragment(apiKeyFragment));
        }

        private static IUploadProcessedNotification StubNotification()
        {
            return new UploadProcessedNotification
            {
                Email = "foo@meow.cat"
            };
        }

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new GovNotifyNotificationsGateway();
            _govNotifyUrl = Environment.GetEnvironmentVariable("GOV_NOTIFY_URL");
            _govNotifyApiKey = Environment.GetEnvironmentVariable("GOV_NOTIFY_API_KEY");

            SetupEnvironmentVariables("http://localhost:3000/", "cafe");

            _simulator = new FluentSimulator("http://localhost:3000/");
            _simulator.Post("/v2/notifications/email").Responds("{}");
            _simulator.Start();
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", _govNotifyUrl);
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY", _govNotifyApiKey);
            _simulator.Stop();
        }


        [TestCase("http://localhost:4000/")]
        [TestCase("http://localhost:5000/")]
        public async Task ItUsesTheGovNotifyUrlEnvironmentVariable(string baseUrl)
        {
            SetupEnvironmentVariables(baseUrl, "cafe");

            var simulator = new FluentSimulator(baseUrl);
            simulator.Start();
            simulator.Post("/v2/notifications/email").Responds("{}");

            await _classUnderTest.SendUploadProcessedNotification(StubNotification(), CancellationToken.None);

            simulator.ReceivedRequests[0].Url.Should().Be($"{baseUrl}v2/notifications/email");
            simulator.Stop();
        }

        [TestCase("abcd")]
        [TestCase("cafe")]
        public void ItUsesTheGovNotifyApiKeyEnvironmentVariable(string apiKeyFragment)
        {
            SetupEnvironmentVariables("http://localhost:3000/", apiKeyFragment);

            Assert.DoesNotThrowAsync(async () =>
                await _classUnderTest.SendUploadProcessedNotification(StubNotification(), CancellationToken.None));
        }

        [TestCase("cat@meow.com")]
        [TestCase("dog@woof.com")]
        public async Task ItPassesTheEmailsToTheNotifyApi(string email)
        {
            UploadProcessedNotification notification = new UploadProcessedNotification
            {
                Email = email
            };

            await _classUnderTest.SendUploadProcessedNotification(notification, CancellationToken.None);

            NotifyRequest notifyRequest = _simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

            notifyRequest.email_address.Should().Be(email);
        }
    }
}
