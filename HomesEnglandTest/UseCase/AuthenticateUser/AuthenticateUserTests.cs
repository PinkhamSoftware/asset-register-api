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
using NSubstitute;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.AuthenticateUser
{
    public class AuthenticateUserTests
    {
        private static IAuthenticateUser _classUnderTest;
        private static IOneTimeAuthenticationTokenCreator _tokenCreatorSpy;
        private static IOneTimeLinkNotifier _notifierSpy;

        private string _authorisedEmails;

        public AuthenticateUserTests()
        {
            _tokenCreatorSpy = Substitute.For<IOneTimeAuthenticationTokenCreator>();
            _notifierSpy = Substitute.For<IOneTimeLinkNotifier>();
            _classUnderTest = new AuthenticateUserUseCase(_tokenCreatorSpy, _notifierSpy);
        }

        [SetUp]
        public void SetUp()
        {
            _authorisedEmails = Environment.GetEnvironmentVariable("EMAIL_WHITELISTS");
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
                Email = invalidEmail
            };
        }

        private static async Task ExpectNotifierGatewayToHaveReceived(string validEmail, string createdTokenString)
        {
            await _notifierSpy.Received()
                .SendOneTimeLinkAsync(Arg.Is<IOneTimeLinkNotification>(req =>
                    req.Email == validEmail && req.Token == createdTokenString));
        }

        private class GivenEmailAddressIsNotAllowed : AuthenticateUserTests
        {
            private class GivenSingleEmailInWhitelist : GivenEmailAddressIsNotAllowed
            {
                [TestCase("test@test.com")]
                [TestCase("meow@cat.com")]
                public async Task ItDoesNotCallTheTokenCreatorGateway(string invalidEmail)
                {
                    SetEmailWhitelist($"mark-as-invalid-{invalidEmail}");
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(invalidEmail);

                    await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    await _tokenCreatorSpy.DidNotReceive()
                        .CreateAsync(Arg.Is<IAuthenticationToken>(req => req.Email == invalidEmail));
                }

                [TestCase("test@test.com")]
                [TestCase("meow@cat.com")]
                public async Task ItMarksTheResponseAsUnauthorised(string invalidEmail)
                {
                    SetEmailWhitelist($"mark-as-invalid-{invalidEmail}");
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(invalidEmail);

                    AuthenticateUserResponse response =
                        await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    response.Authorised.Should().BeFalse();
                }
            }
        }

        private class GivenEmailAddressIsAllowed : AuthenticateUserTests
        {
            private class GivenSingleEmailInWhitelist : GivenEmailAddressIsAllowed
            {
                [TestCase("test@test.com")]
                [TestCase("cat@meow.com")]
                public async Task ItCallsTheTokenCreatorGateway(string validEmail)
                {
                    SetEmailWhitelist(validEmail);
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    await _tokenCreatorSpy.Received()
                        .CreateAsync(Arg.Is<IAuthenticationToken>(req => req.Email == validEmail));
                }

                [TestCase("test@test.com", "token123")]
                [TestCase("cat@meow.com", "anotherToken456")]
                public async Task ItPassesTheEmailAndTokenCreatedToTheNotifierGateway(string validEmail,
                    string createdTokenString)
                {
                    SetEmailWhitelist(validEmail);
                    IAuthenticationToken createdToken = new AuthenticationToken
                    {
                        Email = validEmail,
                        Token = createdTokenString
                    };

                    _tokenCreatorSpy.CreateAsync(Arg.Any<IAuthenticationToken>()).Returns(createdToken);
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
                    await ExpectNotifierGatewayToHaveReceived(validEmail, createdTokenString);
                }

                [TestCase("test@test.com")]
                [TestCase("cat@meow.com")]
                public async Task ItMarksTheResponseAsAuthorised(string validEmail)
                {
                    SetEmailWhitelist(validEmail);
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    AuthenticateUserResponse response =
                        await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    response.Authorised.Should().BeTrue();
                }
            }

            private class GivenMultipleEmailsInWhitelist : GivenEmailAddressIsAllowed
            {
                [TestCase("cow@moo.com")]
                [TestCase("test@example.com")]
                public async Task ItCallsTheTokenCreatorGateway(string validEmail)
                {
                    SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    await _tokenCreatorSpy.Received()
                        .CreateAsync(Arg.Is<IAuthenticationToken>(req => req.Email == validEmail));
                }

                [TestCase("test@test.com", "token123")]
                [TestCase("cat@meow.com", "anotherToken456")]
                public async Task ItPassesTheEmailAndTokenCreatedToTheNotifierGateway(string validEmail,
                    string createdTokenString)
                {
                    SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
                    IAuthenticationToken createdToken = new AuthenticationToken
                    {
                        Email = validEmail,
                        Token = createdTokenString
                    };

                    _tokenCreatorSpy.CreateAsync(Arg.Any<IAuthenticationToken>()).Returns(createdToken);
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
                    await ExpectNotifierGatewayToHaveReceived(validEmail, createdTokenString);
                }

                [TestCase("test@test.com")]
                [TestCase("cat@meow.com")]
                public async Task ItMarksTheResponseAsAuthorised(string validEmail)
                {
                    SetEmailWhitelist($"dog@woof.com;{validEmail};duck@quack.com");
                    AuthenticateUserRequest request = CreateUseCaseRequestForEmail(validEmail);

                    AuthenticateUserResponse response =
                        await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

                    response.Authorised.Should().BeTrue();
                }
            }
        }
    }
}
