using System.Collections.Generic;
using HomesEngland.UseCase.Models;

namespace HomesEngland.UseCase.CalculateAssetAggregates.Models
{
    public class CalculateAssetAggregateResponse:IResponse<AssetAggregatesOutputModel>
    {
        public AssetAggregatesOutputModel AssetAggregates { get; set; }

        public IList<AssetAggregatesOutputModel> ToCsv()
        {
            return new List<AssetAggregatesOutputModel>{AssetAggregates};
        }
    }
}
