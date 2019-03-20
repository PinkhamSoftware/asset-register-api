using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.JWT;
using HomesEngland.UseCase.GetAccessToken;
using JWT;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace HomesEngland.Gateway.Test.JWT
{
    public class JwtAccessTokenGatewayTests
    {
        private class DateTimeProviderFake : IDateTimeProvider
        {
            public DateTime ToReturn { private get; set; }

            public DateTimeProviderFake()
            {
                ToReturn = DateTime.Now;
            }

            public DateTime GetNow()
            {
                return ToReturn;
            }
        }

        private DateTimeProviderFake _dateTimeProviderFake;
        private IAccessTokenCreator _accessTokenCreator;
        private string _secret;

        [SetUp]
        public void SetUp()
        {
            _dateTimeProviderFake = new DateTimeProviderFake();
            _accessTokenCreator = new JwtAccessTokenGateway();
            _secret = Environment.GetEnvironmentVariable("HmacSecret");
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("HmacSecret", _secret);
        }

        [TestCase("Shh its a secret")]
        [TestCase("Dont tell anyone")]
        public async Task GivenCreatingToken_SignItWithHmacSecret(string hmacSecret)
        {
            Environment.SetEnvironmentVariable("HmacSecret", hmacSecret);
            IAccessToken token = await _accessTokenCreator.CreateAsync("stub@stub.com", CancellationToken.None);

            Assert.DoesNotThrow(() => ValidateTokenWithSecret(token, hmacSecret));
        }

        [TestCase("cat@cat.com")]
        [TestCase("dog@dog.com")]
        public async Task GivenCreatingToken_AddTheEmailToTheClaims(string email)
        {
            Environment.SetEnvironmentVariable("HmacSecret", "its a super duper secret");
            IAccessToken token = await _accessTokenCreator.CreateAsync(email, CancellationToken.None);

            AssertTokenContainsEmail(token, email);
        }

        private static void AssertTokenContainsEmail(IAccessToken token, string email)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            IEnumerable<Claim> claims = handler.ReadJwtToken(token.Token).Claims;

            claims.Should().Contain(claim => claim.Type.Equals("email"));
            claims.First(claim => claim.Type.Equals("email")).Value.Should().BeEquivalentTo(email);
        }

        [Test]
        public async Task GivenCreatingToken_ItExpiresAfterEightHours()
        {
            var secret = "super duper secret";
            Environment.SetEnvironmentVariable("HmacSecret", secret);
            IAccessToken token = await _accessTokenCreator.CreateAsync("stub@stub.com", CancellationToken.None);

            var validatedToken = ValidateTokenWithSecret(token, secret);

            validatedToken.ValidTo.Should().BeAfter(DateTime.UtcNow.AddHours(7.9));
            validatedToken.ValidTo.Should().BeBefore(DateTime.UtcNow.AddHours(8.1));
        }

        private static SecurityToken ValidateTokenWithSecret(IAccessToken token, string hmacSecret)
        {
            var verifier = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hmacSecret)),
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false
            };

            verifier.ValidateToken(token.Token, validationParameters, out var validatedToken);

            return validatedToken;
        }
    }
}
