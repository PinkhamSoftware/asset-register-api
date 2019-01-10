using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.Notifications;
using HomesEngland.UseCase.AuthenticateUser;
using HomesEngland.UseCase.AuthenticateUser.Impl;
using HomesEngland.UseCase.AuthenticateUser.Models;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.AuthenticateUser
{
    public class AuthenticateUserTests
    {
        private IAuthenticateUser _classUnderTest;
        private Mock<IOneTimeAuthenticationTokenCreator> _tokenCreatorSpy;
        private Mock<IOneTimeLinkNotifier> _notifierSpy;

        private string _authorisedEmails;

        [SetUp]
        public void SetUp()
        {
            _authorisedEmails = Environment.GetEnvironmentVariable("EMAIL_WHITELISTS");
            _tokenCreatorSpy = new Mock<IOneTimeAuthenticationTokenCreator>();
            _notifierSpy = new Mock<IOneTimeLinkNotifier>();
            _classUnderTest = new AuthenticateUserUseCase(_tokenCreatorSpy.Object, _notifierSpy.Object);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("EMAIL_WHITELIST", _authorisedEmails);
        }

        private static void SetEmailWhitelist(string validEmail)
        {
            Environment.SetEnvironmentVariable("EMAIL_WHITELIST", validEmail);
        }

        private static AuthenticateUserRequest CreateUseCaseRequestForEmail(string invalidEmail)
        {
            return new AuthenticateUserRequest
            {
                Email = invalidEmail,
                Url = ""
            };
        }

        private void ExpectNotifierGatewayToHaveReceived(string validEmail, string createdTokenString)
        {
            _notifierSpy.Verify(s =>
                s.SendOneTimeLinkAsync(NotificationWithExpectedEmailAndToken(validEmail, createdTokenString)));
        }

        private static IOneTimeLinkNotification NotificationWithExpectedEmailAndToken(string validEmail,
            string createdTokenString)
        {
            return It.Is<IOneTimeLinkNotification>(notification =>
                notification.Email == validEmail && notification.Token == createdTokenString);
        }

        private void StubTokenCreator(string email, string token)
        {
            AuthenticationToken authenticationToken = new AuthenticationToken
            {
                Email = email, Token = token
            };

            _tokenCreatorSpy.Setup(s => s.CreateAsync(It.IsAny<IAuthenticationToken>()))
                .ReturnsAsync(authenticationToken);
        }

        private void ExpectTokenCreatorToHaveBeenCalledWith(string validEmail)
        {
            _tokenCreatorSpy.Verify(s =>
                s.CreateAsync(It.Is<IAuthenticationToken>(req => req.Email == validEmail)));
        }


        [TestCase("test@test.com")]
        [TestCase("meow@cat.com")]
        public async Task GivenEmailAddressIsNotAllowed_ItDoesNotCallTheTokenCreator(
            string invalidEmail)
        {
            SetEmailWhitelist($"mark-as-invalid-{invalidEmail}");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(invalidEmail);

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _tokenCreatorSpy.Verify(s => s.CreateAsync(It.IsAny<IAuthenticationToken>()), Times.Never());
        }

        [TestCase("test@test.com")]
        [TestCase("meow@cat.com")]
        public async Task GivenEmailAddressIsNotAllowed_ItReturnsUnauthorised(
            string invalidEmail)
        {
            SetEmailWhitelist($"mark-as-invalid-{invalidEmail}");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(invalidEmail);

            AuthenticateUserResponse response =
                await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeFalse();
        }

        [TestCase("test@test.com")]
        [TestCase("cat@meow.com")]
        public async Task GivenEmailAddressIsAllowed_WithASingleEmailInTheWhitelist_ItCallsTheTokenCreatorGateway(
            string validEmail)
        {
            SetEmailWhitelist(validEmail);
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, "stub");

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            ExpectTokenCreatorToHaveBeenCalledWith(validEmail);
        }

        [TestCase("test@test.com")]
        [TestCase("cat@meow.com")]
        public async Task GivenEmailAddressIsAllowed_WithMultipleEmailsInTheWhitelist_ItCallsTheTokenCreatorGateway(
            string validEmail)
        {
            SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, "stub");

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            ExpectTokenCreatorToHaveBeenCalledWith(validEmail);
        }


        [TestCase("http://meow.cat")]
        [TestCase("http://woof.dog")]
        public async Task
            GivenEmailAddressIsAllowed_WithASingleEmailInTheWhitelist_ItPassesTheUrlToTheNotifierGateway(string url)
        {
            SetEmailWhitelist("test@test.com");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail("test@test.com");
            request.Url = url;
            StubTokenCreator("test@test.com", "stub");

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _notifierSpy.Verify(s => s.SendOneTimeLinkAsync(It.Is<OneTimeLinkNotification>(n => n.Url == url)));
        }

        [TestCase("test@test.com", "token123")]
        [TestCase("cat@meow.com", "anotherToken456")]
        public async Task
            GivenEmailAddressIsAllowed_WithASingleEmailInTheWhitelist_ItPassesTheEmailAndTokenCreatedToTheNotifierGateway(
                string validEmail, string createdTokenString)
        {
            SetEmailWhitelist(validEmail);
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, createdTokenString);

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            ExpectNotifierGatewayToHaveReceived(validEmail, createdTokenString);
        }

        [TestCase("test@test.com", "token123")]
        [TestCase("cat@meow.com", "anotherToken456")]
        public async Task
            GivenEmailAddressIsAllowed_WithMultipleEmailInTheWhitelist_ItPassesTheEmailAndTokenCreatedToTheNotifierGateway(
                string validEmail, string createdTokenString)
        {
            SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, createdTokenString);

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            ExpectNotifierGatewayToHaveReceived(validEmail, createdTokenString);
        }

        [TestCase("test@test.com")]
        [TestCase("cat@meow.com")]
        public async Task
            GivenEmailAddressIsAllowed_WithASingleEmailInTheWhitelist_ItReturnsAuthorised(string validEmail)
        {
            SetEmailWhitelist(validEmail);
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, "stub");

            AuthenticateUserResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeTrue();
        }

        [TestCase("test@test.com")]
        [TestCase("cat@meow.com")]
        public async Task
            GivenEmailAddressIsAllowed_WithMultipleEmailsInTheWhitelist_ItReturnsAuthorised(string validEmail)
        {
            SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
            AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);
            StubTokenCreator(validEmail, "stub");

            AuthenticateUserResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeTrue();
        }
    }
}
