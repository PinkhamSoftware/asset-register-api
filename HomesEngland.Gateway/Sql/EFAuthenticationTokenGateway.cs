using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;

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

            using (var context = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(_databaseUrl).Options))
            {
                context.Add(tokenEntity);
                context.SaveChanges();
                token.Id = tokenEntity.Id;
                IAuthenticationToken foundAsset = context.AuthenticationTokens.Find(tokenEntity.Id);
                return Task.FromResult(foundAsset);
            }
        }

        public Task<IAuthenticationToken> ReadAsync(string token, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(_databaseUrl).Options))
            {
                IAuthenticationToken foundToken = context.AuthenticationTokens.FirstOrDefault(t => t.Token == token);
                return Task.FromResult(foundToken);
            }
        }

        public Task DeleteAsync(string token, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(_databaseUrl).Options))
            {
                AuthenticationTokenEntity foundToken = context.AuthenticationTokens.First(t => t.Token == token);
                context.AuthenticationTokens.Remove(foundToken);
                return Task.FromResult(context.SaveChanges());
            }
        }
    }
}
