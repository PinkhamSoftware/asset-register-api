using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.Assets;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;

namespace HomesEngland.Gateway.Sql
{
    public class EFAssetGateway: AssetRegisterContext, IAssetReader, IAssetCreator, IAssetSearcher, IAssetAggregator
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
            var queryable = FilterQueryable(searchRequest);

            queryable = queryable.Skip(searchRequest.Page.Value * searchRequest.PageSize.Value)
                                 .Take(searchRequest.PageSize.Value);

            IEnumerable<IAsset> results = await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
            
            int totalCount = await FilterQueryable(searchRequest).Select(s => s.Id).CountAsync(cancellationToken).ConfigureAwait(false);
            
            IPagedResults<IAsset> pagedResults = new PagedResults<IAsset>
            {
                Results = results.ToList(),
                TotalCount = totalCount,
                NumberOfPages = (int)Math.Ceiling(totalCount / (decimal)searchRequest.PageSize.Value)
            };

            return pagedResults;
        }

        private IQueryable<DapperAsset> FilterQueryable(IAssetPagedSearchQuery searchRequest)
        {
            IQueryable<DapperAsset> queryable = Assets;
            if (!string.IsNullOrEmpty(searchRequest?.Address) && !string.IsNullOrWhiteSpace(searchRequest?.Address))
                queryable = queryable.Where(w => w.Address.Contains(searchRequest.Address));
            if (searchRequest?.SchemeId != null && searchRequest.SchemeId > 0)
                queryable = queryable.Where(w => w.SchemeId.Equals(searchRequest.SchemeId));
            return queryable;
        }

        public Task<IAssetAggregation> Aggregate(IAssetSearchQuery searchRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
