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

        public Task<IAsset> CreateAsync(IAsset entity)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            var dapperAsset = new DapperAsset(entity);
            var entry = Entry<DapperAsset>(dapperAsset).State = EntityState.Added;

            SaveChanges();
            return Task.FromResult(entity);
        }

        public Task<IAsset> ReadAsync(int index)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            IAsset entity = Find<DapperAsset>(index);

            return Task.FromResult(entity);
        }

        public Task<IPagedResults<IAsset>> Search(IAssetPagedSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            var queryable = GenerateFilteringCriteria(searchRequest);

            queryable = queryable.Skip((searchRequest.Page.Value -1) * searchRequest.PageSize.Value)
                                 .Take(searchRequest.PageSize.Value);

            IEnumerable<IAsset> results = queryable.ToList();

            int totalCount = GenerateFilteringCriteria(searchRequest).Select(s => s.Id).Count();
            
            IPagedResults<IAsset> pagedResults = new PagedResults<IAsset>
            {
                Results = results.ToList(),
                TotalCount = totalCount,
                NumberOfPages = (int)Math.Ceiling(totalCount / (decimal)searchRequest.PageSize.Value)
            };

            return Task.FromResult(pagedResults);
        }

        private IQueryable<DapperAsset> GenerateFilteringCriteria(IAssetSearchQuery searchRequest)
        {
            IQueryable<DapperAsset> queryable = Assets;
            if (!string.IsNullOrEmpty(searchRequest.Address) && !string.IsNullOrWhiteSpace(searchRequest.Address))
            {
                queryable = queryable.Where(w => EF.Functions.Like(w.Address.ToLower(), $"%{searchRequest.Address}%".ToLower()));
            }

            if (searchRequest.SchemeId.HasValue && searchRequest?.SchemeId.Value > 0)
            {
                queryable = queryable.Where(w =>w.SchemeId.HasValue && w.SchemeId == searchRequest.SchemeId.Value);
            }

            return queryable;
        }

        public Task<IAssetAggregation> Aggregate(IAssetSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            var filteringCriteria = GenerateFilteringCriteria(searchRequest);

            var query = filteringCriteria.ToList();

            var aggregatedData = filteringCriteria.Select(s => new
            {
                AssetValue = s.AgencyFairValue,
                MoneyPaidOut = s.AgencyEquityLoan,
                SchemeId = s.SchemeId,
            }).GroupBy(g => g.SchemeId).Select(g => g.First()).ToList();

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
