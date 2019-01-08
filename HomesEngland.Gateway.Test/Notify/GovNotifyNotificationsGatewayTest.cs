using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentSim;
using HomesEngland.Domain;
using HomesEngland.Gateway.Notifications;
using HomesEngland.Gateway.Notify;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test.Notify
{
    public class GovNotifyNotificationsGatewayTest
    {
        private FluentSimulator _simulator;
        private string _govNotifyUrl;
        private string _govNotifyApiKey;
        private IOneTimeLinkNotifier _classUnderTest;

        private class NotifyRequest
        {
            public string email_address { get; set; }
            public string template_id { get; set; }
            public NotifyPersonalisation personalisation { get; set; }
        }

        private class NotifyPersonalisation
        {
            public string token { get; set; }
        }

        private static string BuildApiKeyFromFragment(string fragment)
        {
            return
                $"{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}-{fragment}{fragment}-{fragment}-{fragment}-{fragment}-{fragment}{fragment}{fragment}";
        }

        private static void SetupEnvironmentVariables(string notifyUrl, string apiKeyFragment)
        {
            Environment.SetEnvironmentVariable("GOV_NOTIFY_URL", notifyUrl);
            Environment.SetEnvironmentVariable("GOV_NOTIFY_API_KEY", BuildApiKeyFromFragment(apiKeyFragment));
        }

        private static IOneTimeLinkNotification StubNotification()
        {
            return new OneTimeLinkNotification
            {
                Email = "stub@stub.com"
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

            await _classUnderTest.SendOneTimeLinkAsync(StubNotification());

            simulator.ReceivedRequests[0].Url.Should().Be($"{baseUrl}v2/notifications/email");
            simulator.Stop();
        }

        [TestCase("abcd")]
        [TestCase("cafe")]
        public void ItUsesTheGovNotifyApiKeyEnvironmentVariable(string apiKeyFragment)
        {
            SetupEnvironmentVariables("http://localhost:3000/", apiKeyFragment);

            Assert.DoesNotThrowAsync(async () => await _classUnderTest.SendOneTimeLinkAsync(StubNotification()));
        }

        [TestCase("cat@meow.com")]
        [TestCase("dog@woof.com")]
        public async Task ItPassesTheEmailsToTheNotifyApi(string email)
        {
            OneTimeLinkNotification notification = new OneTimeLinkNotification
            {
                Email = email,
                Token = "stub"
            };

            await _classUnderTest.SendOneTimeLinkAsync(notification);

            NotifyRequest notifyRequest = _simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

            notifyRequest.email_address.Should().Be(email);
        }

        [TestCase("tokenOne")]
        [TestCase("tokenTwo")]
        public async Task ItPassesTheTokensToTheNotifyApi(string token)
        {
            OneTimeLinkNotification notification = new OneTimeLinkNotification
            {
                Email = "stub@stub.com",
                Token = token
            };

            await _classUnderTest.SendOneTimeLinkAsync(notification);

            NotifyRequest notifyRequest = _simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

            notifyRequest.personalisation.token.Should().Be(token);
        }

        [Test]
        public async Task ItPassesATemplateToTheNotifyApi()
        {
            await _classUnderTest.SendOneTimeLinkAsync(StubNotification());

            NotifyRequest notifyRequest = _simulator.ReceivedRequests[0].BodyAs<NotifyRequest>();

            notifyRequest.template_id.Should().NotBeNullOrEmpty();
            notifyRequest.template_id.Should().NotBeNullOrWhiteSpace();
        }
    }
}
