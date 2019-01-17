using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Domain.Impl;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;

namespace HomesEngland.Gateway.Sql
{
    public class EFAssetGateway : IGateway<IAsset, int>, IAssetReader, IAssetCreator, IAssetSearcher, IAssetAggregator, IBulkAssetCreator
    {
        private readonly string _databaseUrl; 

        public EFAssetGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public Task<IAsset> CreateAsync(IAsset entity)
        {
            var assetEntity = new AssetEntity(entity);

            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                context.Add(assetEntity);
                context.SaveChanges();
                entity.Id = assetEntity.Id;
                IAsset foundAsset = context.Assets.Find(assetEntity.Id);
                return Task.FromResult(foundAsset);
            }
        }

        public Task<IAsset> ReadAsync(int index)
        {
            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                IAsset entity = context.Assets.Find(index);

                return Task.FromResult(entity);
            }
        }

        public Task<IPagedResults<IAsset>> Search(IAssetPagedSearchQuery searchRequest,
            CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                var queryable = GenerateFilteringCriteria(context, searchRequest);

                queryable = queryable.Skip((searchRequest.Page.Value - 1) * searchRequest.PageSize.Value)
                    .Take(searchRequest.PageSize.Value);

                IEnumerable<IAsset> results = queryable.ToList();

                int totalCount = GenerateFilteringCriteria(context, searchRequest).Select(s => s.Id).Count();
                IPagedResults<IAsset> pagedResults = new PagedResults<IAsset>
                {
                    Results = results.ToList(),
                    TotalCount = totalCount,
                    NumberOfPages = (int) Math.Ceiling(totalCount / (decimal) searchRequest.PageSize.Value)
                };

                return Task.FromResult(pagedResults);
            }
        }

        private IQueryable<AssetEntity> GenerateFilteringCriteria(AssetRegisterContext context,
            IAssetSearchQuery searchRequest)
        {
            IQueryable<AssetEntity> queryable = context.Assets;
            if (!string.IsNullOrEmpty(searchRequest.Address) && !string.IsNullOrWhiteSpace(searchRequest.Address))
            {
                queryable = queryable.Where(w =>
                    EF.Functions.Like(w.Address.ToLower(), $"%{searchRequest.Address}%".ToLower()));
            }

            if (searchRequest.SchemeId.HasValue && searchRequest?.SchemeId.Value > 0)
            {
                queryable = queryable.Where(w => w.SchemeId.HasValue && w.SchemeId == searchRequest.SchemeId.Value);
            }

            queryable = queryable.OrderByDescending(w => w.SchemeId);

            return queryable;
        }

        public Task<IAssetAggregation> Aggregate(IAssetSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                var filteringCriteria = GenerateFilteringCriteria(context, searchRequest);

                var aggregatedData = filteringCriteria.Select(s => new
                {
                    AssetValue = s.AgencyFairValue,
                    MoneyPaidOut = s.AgencyEquityLoan,
                    SchemeId = s.SchemeId,
                });

                decimal? uniqueCount = aggregatedData?.Select(w => w.SchemeId).Distinct().Count();
                decimal? moneyPaidOut = aggregatedData?.Select(w => w.MoneyPaidOut).Sum(s => s);
                decimal? assetValue = aggregatedData?.Select(w => w.AssetValue).Sum(s => s);

                IAssetAggregation assetAggregates = new AssetAggregation
                {
                    UniqueRecords = uniqueCount,
                    AssetValue = assetValue,
                    MoneyPaidOut = moneyPaidOut,
                    MovementInAssetValue = assetValue - moneyPaidOut
                };
                return Task.FromResult(assetAggregates);
            }
        }

        public async Task<IList<IAsset>> BulkCreateAsync(IList<IAsset> entities, CancellationToken cancellationToken)
        {
            List<AssetEntity> assetEntities = entities.Select(s=> new AssetEntity(s)).ToList();

            using (var context = new AssetRegisterContext(_databaseUrl))
            {
                await context.AddRangeAsync(assetEntities, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                var t = assetEntities.Select(s => new Asset(s) as IAsset);
                IList<IAsset> list = t.ToList();
                return list;
            }
        }
    }
}
