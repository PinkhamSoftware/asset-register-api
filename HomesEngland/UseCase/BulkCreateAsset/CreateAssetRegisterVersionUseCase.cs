using System;
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
    public class CreateAssetRegisterVersionUseCase : ICreateAssetRegisterVersionUseCase
    {
        private readonly IAssetRegisterVersionCreator _assetRegisterVersionCreator;

        public CreateAssetRegisterVersionUseCase(IAssetRegisterVersionCreator assetRegisterVersionCreator)
        {
            _assetRegisterVersionCreator = assetRegisterVersionCreator;
        }

        public async Task<IList<CreateAssetResponse>> ExecuteAsync(IList<CreateAssetRequest> requests, CancellationToken cancellationToken)
        {
            List<IAsset> assets = requests.Select(s => new Asset(s) as IAsset).ToList();

            IAssetRegisterVersion assetRegisterVersion = new AssetRegisterVersion
            {
                Assets = assets,
                ModifiedDateTime = DateTime.UtcNow
            };
            Console.WriteLine($" Inserting AssetRegisterVersion Start {DateTime.UtcNow.TimeOfDay.ToString("g")}");
            var result = await _assetRegisterVersionCreator.CreateAsync(assetRegisterVersion, cancellationToken).ConfigureAwait(false);
            if (result == null)
                throw new CreateAssetRegisterVersionException();
            Console.WriteLine($" Inserting AssetRegisterVersion Finish {DateTime.UtcNow.TimeOfDay.ToString("g")}");
            List<CreateAssetResponse> responses = result.Assets.Select(s => new CreateAssetResponse
            {
                Asset = new AssetOutputModel(s)
            }).ToList();

            return responses;
        }
    }
}
