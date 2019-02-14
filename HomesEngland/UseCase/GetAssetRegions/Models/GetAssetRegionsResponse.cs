using System.Collections.Generic;
using HomesEngland.Domain;
using HomesEngland.UseCase.Models;

namespace HomesEngland.UseCase.GetAssetRegions.Models
{
    public class GetAssetRegionsResponse:IResponse<AssetRegion>
    {
        public IList<AssetRegion> Regions { get; set; }
        public IList<AssetRegion> ToCsv()
        {
            return Regions;
        }
    }
}
