using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.AccessTokens;
using HomesEngland.UseCase.ImportAssets.Impl;
using JWT;
using Microsoft.IdentityModel.Tokens;

namespace HomesEngland.Gateway.JWT
{
    public class JwtAccessTokenGateway : IAccessTokenCreator
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtAccessTokenGateway()
        {
            _dateTimeProvider = new UtcDateTimeProvider();
        }

        public Task<IAccessToken> CreateAsync(string email, CancellationToken cancellationToken)
        {
            AccessToken accessToken = new AccessToken
            {
                Token = GenerateTokenString(email)
            };

            return Task.FromResult<IAccessToken>(accessToken);
        }

        private string GenerateTokenString(string email)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            Claim emailClaim = new Claim("email", email);

            List<Claim> claims = new List<Claim> {emailClaim};

            string tokenString = tokenHandler.WriteToken(tokenHandler.CreateJwtSecurityToken(
                signingCredentials: GetSigningCredentials(),
                expires: GetExpiryTime(),
                subject: new ClaimsIdentity(claims)
            ));

            return tokenString;
        }

        private DateTime GetExpiryTime()
        {
            DateTime expiryTime = _dateTimeProvider.GetNow().AddHours(8);
            return expiryTime;
        }

        private static SigningCredentials GetSigningCredentials()
        {
            string key = Environment.GetEnvironmentVariable("HmacSecret");
            Console.WriteLine(key);
            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha512);
            return signingCredentials;
        }
    }
}
