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
    public class EFAssetRegisterVersionGateway : IAssetRegisterVersionCreator, IAssetRegisterVersionSearcher
    {
        private readonly string _databaseUrl;

        public EFAssetRegisterVersionGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public async Task<IAssetRegisterVersion> CreateAsync(IAssetRegisterVersion assetRegisterVersion, CancellationToken cancellationToken)
        {
            AssetRegisterVersionEntity assetRegisterVersionEntity = new AssetRegisterVersionEntity(assetRegisterVersion);

            assetRegisterVersionEntity.Assets = assetRegisterVersionEntity.Assets?.Select(s =>
            {
                s.AssetRegisterVersion = assetRegisterVersionEntity;
                return s;
            }).ToList();

            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                await context.AssetRegisterVersions.AddAsync(assetRegisterVersionEntity, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                var result = new AssetRegisterVersion
                {
                    Id = assetRegisterVersionEntity.Id,
                    ModifiedDateTime = assetRegisterVersionEntity.ModifiedDateTime,
                    Assets = assetRegisterVersionEntity.Assets?.Select(s=> new Asset(s) as IAsset).ToList()
                };
                return result;
            }
        }

        public Task<IPagedResults<IAssetRegisterVersion>> Search(IPagedQuery searchRequest, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                IQueryable<AssetRegisterVersionEntity> queryable = context.AssetRegisterVersions;

                queryable = queryable.OrderByDescending(o=> o.Id)
                            .Skip((searchRequest.Page.Value - 1) * searchRequest.PageSize.Value)
                            .Take(searchRequest.PageSize.Value);

                IEnumerable<AssetRegisterVersionEntity> results = queryable.ToList();

                int totalCount = context.AssetRegisterVersions.Select(s => s.Id).Count();
                IPagedResults<IAssetRegisterVersion> pagedResults = new PagedResults<IAssetRegisterVersion>
                {
                    Results = results.Select(s=> new AssetRegisterVersion
                    {
                        Id = s.Id,
                        ModifiedDateTime = s.ModifiedDateTime
                    } as IAssetRegisterVersion).ToList(),
                    TotalCount = totalCount,
                    NumberOfPages = (int)Math.Ceiling(totalCount / (decimal)searchRequest.PageSize.Value)
                };
                return Task.FromResult(pagedResults);
            }
        }
    }
}
