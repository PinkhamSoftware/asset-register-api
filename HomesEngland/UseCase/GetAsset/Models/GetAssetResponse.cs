using System.Collections.Generic;
using HomesEngland.UseCase.Models;
using HomesEngland.UseCase.SearchAsset.Models;

namespace HomesEngland.UseCase.GetAsset.Models
{
    public class GetAssetResponse:IResponse<AssetOutputModel>
    {
        public AssetOutputModel Asset { get; set; }
        public IList<AssetOutputModel> ToCsv()
        {
            return new List<AssetOutputModel>{Asset};
        }
    }
}
