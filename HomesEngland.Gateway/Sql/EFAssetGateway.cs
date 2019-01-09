using System;
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
    public class EFAssetGateway : IGateway<IAsset, int>, IAssetReader, IAssetCreator,
        IAssetSearcher, IAssetAggregator
    {
        private readonly string _databaseUrl; 

        public EFAssetGateway(string databaseUrl)
        {
            _databaseUrl = databaseUrl;
        }

        public Task<IAsset> CreateAsync(IAsset entity)
        {
            //
            var dapperAsset = new AssetEntity(entity);

            using (var context = new AssetRegisterContext())
            {
                context.Add(dapperAsset);
                context.SaveChanges();
                entity.Id = dapperAsset.Id;
                var foundAsset = context.Assets.Find(dapperAsset.Id);
                return Task.FromResult(entity);
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
    }
}
