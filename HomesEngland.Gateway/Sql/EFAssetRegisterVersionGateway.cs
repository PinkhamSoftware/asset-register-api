using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.Gateway.Migrations;
using HomesEngland.UseCase.CreateAssetRegisterVersion.Models;
using Microsoft.EntityFrameworkCore;

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

            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Associate Entities with Asset Register Version");

            assetRegisterVersionEntity.Assets = assetRegisterVersionEntity.Assets?.Select(s =>
            {
                s.AssetRegisterVersion = assetRegisterVersionEntity;
                return s;
            }).ToList();

            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finish Associate Entities with Asset Register Version");

            using (var context = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(_databaseUrl).Options))
            {
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Add async");
                await context.AssetRegisterVersions.AddAsync(assetRegisterVersionEntity).ConfigureAwait(false);
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finish Add async");
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Save Changes async");
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finish Save Changes async");
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Marshall Data");
                IAssetRegisterVersion result = new AssetRegisterVersion
                {
                    Id = assetRegisterVersionEntity.Id,
                    ModifiedDateTime = assetRegisterVersionEntity.ModifiedDateTime,
                    Assets = assetRegisterVersionEntity.Assets?.Select(s=> new Asset(s) as IAsset).ToList()
                };
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finish Marshall Data");
                return result;
            }
        }

        public Task<IPagedResults<IAssetRegisterVersion>> Search(IPagedQuery searchRequest, CancellationToken cancellationToken)
        {
            using (var context = new AssetRegisterContext(new DbContextOptionsBuilder<AssetRegisterContext>().UseSqlServer(_databaseUrl).Options))
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
