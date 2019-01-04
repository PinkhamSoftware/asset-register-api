using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain.Factory;
using HomesEngland.UseCase.CreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.GetAsset.Models;
using HomesEngland.UseCase.ImportAssets.Models;

namespace HomesEngland.UseCase.ImportAssets.Impl
{
    public class ImportAssetsUseCase : IImportAssetsUseCase
    {
        private readonly ICreateAssetUseCase _createAssetUseCase;
        private readonly IFactory<CreateAssetRequest, CsvAsset> _createAssetRequestFactory;

        public ImportAssetsUseCase(ICreateAssetUseCase createAssetUseCase,
            IFactory<CreateAssetRequest, CsvAsset> createAssetRequestFactory)
        {
            _createAssetUseCase = createAssetUseCase;
            _createAssetRequestFactory = createAssetRequestFactory;
        }

        public async Task<ImportAssetsResponse> ExecuteAsync(ImportAssetsRequest request,
            CancellationToken cancellationToken)
        {
            ImportAssetsResponse response = new ImportAssetsResponse
            {
                AssetsImported = new List<AssetOutputModel>()
            };

            foreach (var requestAssetLine in request.AssetLines)
            {
                var createdAsset = await CreateAssetForLine(request, cancellationToken, requestAssetLine);

                response.AssetsImported.Add(createdAsset.Asset);
            }

            return response;
        }

        private async Task<CreateAssetResponse> CreateAssetForLine(ImportAssetsRequest request,
            CancellationToken cancellationToken,
            string requestAssetLine)
        {
            CsvAsset csvAsset = new CsvAsset
            {
                CsvLine = requestAssetLine,
                Delimiter = request.Delimiter
            };

            CreateAssetRequest createAssetRequest = _createAssetRequestFactory.Create(csvAsset);

            var createdAsset = await _createAssetUseCase.ExecuteAsync(createAssetRequest, cancellationToken)
                .ConfigureAwait(false);
            return createdAsset;
        }
    }
}
