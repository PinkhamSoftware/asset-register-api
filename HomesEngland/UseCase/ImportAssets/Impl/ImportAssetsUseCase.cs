using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain.Factory;
using HomesEngland.UseCase.CreateAsset.Models;
using HomesEngland.UseCase.CreateAsset.Models.Factory;
using HomesEngland.UseCase.CreateAssetRegisterVersion;
using HomesEngland.UseCase.ImportAssets.Models;

namespace HomesEngland.UseCase.ImportAssets.Impl
{
    public class ImportAssetsUseCase : IImportAssetsUseCase
    {
        private readonly ICreateAssetRegisterVersionUseCase _createAssetRegisterVersionUseCase;
        private readonly IFactory<CreateAssetRequest, CsvAsset> _createAssetRequestFactory;

        public ImportAssetsUseCase(ICreateAssetRegisterVersionUseCase createAssetRegisterVersionUseCase, IFactory<CreateAssetRequest, CsvAsset> createAssetRequestFactory)
        {
            _createAssetRegisterVersionUseCase = createAssetRegisterVersionUseCase;
            _createAssetRequestFactory = createAssetRequestFactory;
        }

        public async Task<ImportAssetsResponse> ExecuteAsync(ImportAssetsRequest requests, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Creating asset Requests");
            List<CreateAssetRequest> createAssetRequests = new List<CreateAssetRequest>();
            for (int i = 0; i < requests.AssetLines.Count; i++)
            {
                var requestAssetLine = requests.AssetLines.ElementAtOrDefault(i);
                var createAssetRequest = CreateAssetForLine(requests, cancellationToken, requestAssetLine);
                createAssetRequests.Add(createAssetRequest);
                Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Creating asset Request: {i} of {requests.AssetLines.Count}");
            }
            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finished Creating asset Requests");

            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Start Creating AssetRegisterVersion");
            var responses = await _createAssetRegisterVersionUseCase.ExecuteAsync(createAssetRequests, cancellationToken).ConfigureAwait(false);
            Console.WriteLine($"{DateTime.UtcNow.TimeOfDay.ToString("g")}: Finished Creating AssetRegisterVersion");


            ImportAssetsResponse response = new ImportAssetsResponse
            {
                AssetsImported = responses.Select(s=> s.Asset).ToList()
            };

            return response;
        }

        private CreateAssetRequest CreateAssetForLine(ImportAssetsRequest request, CancellationToken cancellationToken, string requestAssetLine)
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
