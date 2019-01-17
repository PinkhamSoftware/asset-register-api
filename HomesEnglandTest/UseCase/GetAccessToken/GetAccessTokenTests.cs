using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.UseCase.GetAccessToken.Impl;
using HomesEngland.UseCase.GetAccessToken.Models;
using Moq;
using NUnit.Framework;

namespace HomesEnglandTest.UseCase.GetAccessToken
{
    [TestFixture]
    public class GetAccessTokenTests
    {
        private GetAccessTokenUseCase _classUnderTest;
        private Mock<IOneTimeAuthenticationTokenReader> _tokenReaderSpy;
        private Mock<IOneTimeAuthenticationTokenDeleter> _tokenDeleterSpy;
        private Mock<IAccessTokenCreator> _accessTokenCreatorSpy;

        [SetUp]
        public void SetUp()
        {
            _accessTokenCreatorSpy = new Mock<IAccessTokenCreator>();
            _tokenReaderSpy = new Mock<IOneTimeAuthenticationTokenReader>();
            _tokenDeleterSpy = new Mock<IOneTimeAuthenticationTokenDeleter>();
            _classUnderTest = new GetAccessTokenUseCase(_tokenReaderSpy.Object, _accessTokenCreatorSpy.Object,
                _tokenDeleterSpy.Object);
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequest_CallTheTokenReaderWithTheToken(string token)
        {
            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _tokenReaderSpy.Verify(e =>
                e.ReadAsync(It.Is<string>(s => s.Equals(token)), It.IsAny<CancellationToken>()));
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequestWithNoneMatchingToken_ReturnUnauthorised(string token)
        {
            _tokenReaderSpy.Setup(e => e.ReadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IAuthenticationToken) null);

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            GetAccessTokenResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeFalse();
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequestMatchingToken_WhenTokenHasExpired_ReturnUnauthorised(string token)
        {
            StubTokenReaderWithExpiredToken(token);

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            GetAccessTokenResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeFalse();
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequestMatchingToken_WhenTokenHasNotExpired_ReturnAuthorised(string token)
        {
            StubTokenReaderWithValidToken(token);
            StubAccessTokenCreator("token");

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            GetAccessTokenResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Authorised.Should().BeTrue();
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequestMatchingToken_WhenTokenIsValid_CreateAccessToken(string token)
        {
            StubTokenReaderWithValidToken(token);
            StubAccessTokenCreator("token");

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _accessTokenCreatorSpy.Verify(e => e.CreateAsync(It.IsAny<CancellationToken>()));
        }

        [TestCase("Meow meow")]
        [TestCase("Woof woof")]
        public async Task GivenRequestMatchingToken_WhenTokenIsValid_ReturnCreatedAccessToken(string createdToken)
        {
            StubTokenReaderWithValidToken("token");
            StubAccessTokenCreator(createdToken);

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = "token"
            };

            GetAccessTokenResponse response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.AccessToken.Should().BeEquivalentTo(createdToken);
        }

        [TestCase("meow")]
        [TestCase("woof")]
        public async Task GivenRequestMatchingToken_WhenTokenIsValid_DeleteToken(string token)
        {
            StubTokenReaderWithValidToken(token);
            StubAccessTokenCreator("fake token");

            GetAccessTokenRequest request = new GetAccessTokenRequest
            {
                Token = token
            };

            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _tokenDeleterSpy.Verify(v =>
                v.DeleteAsync(It.Is<string>(s => s.Equals(token)), It.IsAny<CancellationToken>()));
        }

        private void StubAccessTokenCreator(string createdToken)
        {
            _accessTokenCreatorSpy.Setup(s => s.CreateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AccessToken {Token = createdToken});
        }

        private void StubTokenReaderWithValidToken(string token)
        {
            _tokenReaderSpy.Setup(e => e.ReadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthenticationToken {Token = token, Expiry = DateTime.Now.AddDays(1)});
        }

        private void StubTokenReaderWithExpiredToken(string token)
        {
            _tokenReaderSpy.Setup(e => e.ReadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthenticationToken {Token = token, Expiry = DateTime.Now.Subtract(new TimeSpan(1))});
        }
    }
}
