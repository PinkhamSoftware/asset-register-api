using System;
using System.IdentityModel.Tokens.Jwt;
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

        public Task<IAccessToken> CreateAsync(CancellationToken cancellationToken)
        {
            AccessToken accessToken = new AccessToken
            {
                Token = GenerateTokenString()
            };

            return Task.FromResult<IAccessToken>(accessToken);
        }

        private string GenerateTokenString()
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenString = tokenHandler.WriteToken(tokenHandler.CreateJwtSecurityToken(signingCredentials: GetSigningCredentials(),
                expires: GetExpiryTime()));
            
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
