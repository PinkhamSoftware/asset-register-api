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
    public class EFAssetGateway: AssetRegisterContext, IGateway<IAsset, int>, IAssetReader, IAssetCreator, IAssetSearcher, IAssetAggregator
    {
        public EFAssetGateway(string connectionString) : base(connectionString)
        {

        }

        public EFAssetGateway() : base() { }

        public async Task<IAsset> CreateAsync(IAsset entity)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            var dapperAsset = new DapperAsset(entity);
            var entry = Entry<DapperAsset>(dapperAsset).State = EntityState.Added;
            await SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<IAsset> ReadAsync(int index)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            var entity = await FindAsync<DapperAsset>(index).ConfigureAwait(false);
            return entity;
        }

        public async Task<IPagedResults<IAsset>> Search(IAssetPagedSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            var queryable = GenerateFilteringCriteria(searchRequest);

            queryable = queryable.Skip(searchRequest.Page.Value * searchRequest.PageSize.Value)
                                 .Take(searchRequest.PageSize.Value);

            IEnumerable<IAsset> results = await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
            
            int totalCount = await GenerateFilteringCriteria(searchRequest).Select(s => s.Id).CountAsync(cancellationToken).ConfigureAwait(false);
            
            IPagedResults<IAsset> pagedResults = new PagedResults<IAsset>
            {
                Results = results.ToList(),
                TotalCount = totalCount,
                NumberOfPages = (int)Math.Ceiling(totalCount / (decimal)searchRequest.PageSize.Value)
            };

            return pagedResults;
        }

        private IQueryable<DapperAsset> GenerateFilteringCriteria(IAssetSearchQuery searchRequest)
        {
            IQueryable<DapperAsset> queryable = Assets;
            if (!string.IsNullOrEmpty(searchRequest.Address) && !string.IsNullOrWhiteSpace(searchRequest.Address))
            {
                queryable = queryable.Where(w => w.Address.ToLower().Contains(searchRequest.Address.ToLower()));
            }

            if (searchRequest.SchemeId.HasValue && searchRequest?.SchemeId.Value > 0)
            {
                queryable = queryable.Where(w =>w.SchemeId.HasValue && w.SchemeId == searchRequest.SchemeId.Value);
            }

            return queryable;
        }

        public async Task<IAssetAggregation> Aggregate(IAssetSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            var filteringCriteria = GenerateFilteringCriteria(searchRequest);

            var aggregatedData = await filteringCriteria.Select(s => new
            {
                AssetValue = s.AgencyFairValue,
                MoneyPaidOut = s.AgencyEquityLoan,
                SchemeId = s.SchemeId,
            }).GroupBy(g=> g.SchemeId).Select(g=> g.First()).ToListAsync(cancellationToken).ConfigureAwait(false);

            decimal? uniqueCount = aggregatedData?.Select(w => w.SchemeId).Distinct().Count();
            decimal? moneyPaidOut = aggregatedData?.Select(w => w.MoneyPaidOut).Sum(s => s);
            decimal? assetValue = aggregatedData?.Select(w => w.AssetValue).Sum(s => s);

            var assetAggregates = new AssetAggregation
            {
                UniqueRecords = uniqueCount,
                AssetValue = assetValue,
                MoneyPaidOut = moneyPaidOut,
                MovementInAssetValue = assetValue - moneyPaidOut
            };
            return assetAggregates;
        }
    }
}
