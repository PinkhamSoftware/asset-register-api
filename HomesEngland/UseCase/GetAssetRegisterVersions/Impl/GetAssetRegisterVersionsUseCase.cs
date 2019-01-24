using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.Domain;
using HomesEngland.Gateway;
using HomesEngland.Gateway.AssetRegisterVersions;
using HomesEngland.UseCase.BulkCreateAsset.Models;
using HomesEngland.UseCase.GetAssetRegisterVersions.Models;

namespace HomesEngland.UseCase.GetAssetRegisterVersions.Impl
{
    public class GetAssetRegisterVersionsUseCase: IGetAssetRegisterVersionsUseCase
    {
        private readonly IAssetRegisterVersionSearcher _assetRegisterVersionSearcher;

        public GetAssetRegisterVersionsUseCase(IAssetRegisterVersionSearcher assetRegisterVersionSearcher)
        {
            _assetRegisterVersionSearcher = assetRegisterVersionSearcher;
        }
        public async Task<GetAssetRegisterVersionsResponse> ExecuteAsync(GetAssetRegisterVersionsRequest requests, CancellationToken cancellationToken)
        {
            var query = new PagedQuery();

            if (requests.Page != null) query.Page = requests.Page;
            if (requests.PageSize != null) query.PageSize = requests.PageSize;

            var response = await _assetRegisterVersionSearcher.Search(query, cancellationToken).ConfigureAwait(false) 
               ?? new PagedResults<IAssetRegisterVersion>
               {
                   Results = new List<IAssetRegisterVersion>(),
                   NumberOfPages = 0,
                   TotalCount = 0
               };

            var useCaseReponse = new GetAssetRegisterVersionsResponse
            {
                AssetRegisterVersions = response.Results.Select(s => new AssetRegisterVersionOutputModel(s)).ToList(),
                TotalCount = response.TotalCount,
                Pages = response.NumberOfPages
            };
            return useCaseReponse;
        }
    }
}
