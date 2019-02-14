using System;
using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.GetAssetRegions.Models;

namespace HomesEngland.UseCase.GetAssetRegions.Impl
{
    public class GetAssetRegionsUseCase : IGetAssetRegionsUseCase
    {
        public Task<GetAssetRegionsResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
