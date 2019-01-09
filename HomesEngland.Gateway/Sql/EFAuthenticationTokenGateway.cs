using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;

namespace HomesEngland.Gateway.Sql
{
    public class EFAuthenticationTokenGateway : IAuthenticationGateway
    {
        private readonly string _databaseUrl;
        public EFAuthenticationTokenGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public Task<IAuthenticationToken> CreateAsync(IAuthenticationToken token, CancellationToken cancellationToken)
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

        public Task<IAuthenticationToken> ReadAsync(int index, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                IAuthenticationToken foundAsset = context.AuthenticationTokens.Find(index);
                return Task.FromResult(foundAsset);
            }
        }
    }
}
