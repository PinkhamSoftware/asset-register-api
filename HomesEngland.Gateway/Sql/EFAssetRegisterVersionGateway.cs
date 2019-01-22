using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.BulkCreateAsset.Models;

namespace HomesEngland.Gateway.Sql
{
    public class EFAssetRegisterVersionGateway : IBulkAssetCreator, IAssetRegisterVersionSearcher
    {
        private readonly string _databaseUrl;

        public EFAssetRegisterVersionGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public async Task<IList<IAsset>> BulkCreateAsync(IAssetRegisterVersion assetRegisterVersion, CancellationToken cancellationToken)
        {
            AssetRegisterVersionEntity assetRegisterVersionEntity = new AssetRegisterVersionEntity(assetRegisterVersion);

            assetRegisterVersionEntity.Assets = assetRegisterVersionEntity.Assets.Select(s =>
            {
                s.AssetRegisterVersion = assetRegisterVersionEntity;
                return s;
            }).ToList();

            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                await context.AssetRegisterVersions.AddAsync(assetRegisterVersionEntity, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                var t = assetRegisterVersionEntity.Assets.Select(s => new Asset(s) as IAsset);
                IList<IAsset> list = t.ToList();
                return list;
            }
        }

        public async Task<IPagedResults<IAssetRegisterVersion>> Search(IPagedQuery searchRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
