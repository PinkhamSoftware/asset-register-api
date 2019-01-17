﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace HomesEngland.UseCase.CreateAsset
{
    public class BulkCreateAssetUseCase : IBulkCreateAssetUseCase
    {
        private readonly IBulkAssetCreator _bulkAssetCreator;

        public BulkCreateAssetUseCase(IBulkAssetCreator bulkAssetCreator)
        {
            _bulkAssetCreator = bulkAssetCreator;
        }

        public async Task<IList<CreateAssetResponse>> ExecuteAsync(IList<CreateAssetRequest> requests, CancellationToken cancellationToken)
        {
            List<IAsset> assets = requests.Select(s => new Asset(s) as IAsset).ToList();

            var result = await _bulkAssetCreator.BulkCreateAsync(assets, cancellationToken).ConfigureAwait(false);

            List<CreateAssetResponse> responses = result.Select(s => new CreateAssetResponse
            {
                Asset = new AssetOutputModel(s)
            }).ToList();

            return responses;
        }
    }
}
