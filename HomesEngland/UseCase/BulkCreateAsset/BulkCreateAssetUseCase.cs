﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Exception;
using HomesEngland.Gateway;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.BulkCreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.GetAsset.Models;

namespace HomesEngland.UseCase.BulkCreateAsset
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

            IAssetRegisterVersion assetRegisterVersion = new AssetRegisterVersion
            {
                Assets = assets,
                ModifiedDateTime = DateTime.UtcNow
            };

            var result = await _bulkAssetCreator.BulkCreateAsync(assetRegisterVersion, cancellationToken).ConfigureAwait(false);
            if (result == null || !result.Any())
                throw new BulkCreateAssetException();

            List<CreateAssetResponse> responses = result.Select(s => new CreateAssetResponse
            {
                Asset = new AssetOutputModel(s)
            }).ToList();

            return responses;
        }
    }
}
