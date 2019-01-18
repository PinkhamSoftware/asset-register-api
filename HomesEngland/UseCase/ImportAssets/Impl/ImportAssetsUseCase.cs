using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain.Factory;
using HomesEngland.UseCase.BulkCreateAsset;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.ImportAssets.Models;

namespace HomesEngland.UseCase.ImportAssets.Impl
{
    public class ImportAssetsUseCase : IImportAssetsUseCase
    {
        private readonly IBulkCreateAssetUseCase _bulkCreateAssetUseCase;
        private readonly IFactory<CreateAssetRequest, CsvAsset> _createAssetRequestFactory;

        public ImportAssetsUseCase(IBulkCreateAssetUseCase bulkCreateAssetUseCase, IFactory<CreateAssetRequest, CsvAsset> createAssetRequestFactory)
        {
            _bulkCreateAssetUseCase = bulkCreateAssetUseCase;
            _createAssetRequestFactory = createAssetRequestFactory;
        }

        public async Task<ImportAssetsResponse> ExecuteAsync(ImportAssetsRequest requests, CancellationToken cancellationToken)
        {
            List<CreateAssetRequest> createAssetRequests = new List<CreateAssetRequest>();
            foreach (var requestAssetLine in requests.AssetLines)
            {
                var createAssetRequest = await CreateAssetForLine(requests, cancellationToken, requestAssetLine);
                createAssetRequests.Add(createAssetRequest);
            }

            var responses = await _bulkCreateAssetUseCase.ExecuteAsync(createAssetRequests, cancellationToken).ConfigureAwait(false);

            ImportAssetsResponse response = new ImportAssetsResponse
            {
                AssetsImported = responses.Select(s=> s.Asset).ToList()
            };

            return response;
        }

        private async Task<CreateAssetRequest> CreateAssetForLine(ImportAssetsRequest request, CancellationToken cancellationToken, string requestAssetLine)
        {
            CsvAsset csvAsset = new CsvAsset
            {
                CsvLine = requestAssetLine,
                Delimiter = request.Delimiter
            };

            CreateAssetRequest createAssetRequest = _createAssetRequestFactory.Create(csvAsset);

            return createAssetRequest;
        }
    }
}
