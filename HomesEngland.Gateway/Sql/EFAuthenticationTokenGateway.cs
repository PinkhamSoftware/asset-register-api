using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AuthenticationTokens;
using HomesEngland.Gateway.Migrations;

namespace HomesEngland.Gateway.Sql
{
    public class EFAuthenticationTokenGateway : IOneTimeAuthenticationTokenCreator
    {
        private readonly string _databaseUrl;
        public EFAuthenticationTokenGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public Task<IAuthenticationToken> CreateAsync(IAuthenticationToken token)
        {
            
            var tokenEntity = new AuthenticationTokenEntity(token);

            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                context.Add(tokenEntity);
                context.SaveChanges();
                token.Id = tokenEntity.Id;
                IAuthenticationToken foundAsset = context.AuthenticationTokens.Find(tokenEntity.Id);
                return Task.FromResult(foundAsset);
            }
        }
    }
}
