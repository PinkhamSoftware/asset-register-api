using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomesEngland.Domain;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.Gateway.JWT;
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
            IAccessToken token = await _accessTokenCreator.CreateAsync(CancellationToken.None);

            Assert.DoesNotThrow(() => ValidateTokenWithSecret(token, hmacSecret));
        }

        [Test]
        public async Task GivenCreatingToken_ItExpiresAfterEightHours()
        {
            var secret = "super duper secret";
            Environment.SetEnvironmentVariable("HmacSecret", secret);
            IAccessToken token = await _accessTokenCreator.CreateAsync(CancellationToken.None);

            var validatedToken = ValidateTokenWithSecret(token, secret);

            validatedToken.ValidTo.Should().BeAfter(DateTime.Now.AddHours(7.9));
            validatedToken.ValidTo.Should().BeBefore(DateTime.Now.AddHours(8.1));
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
