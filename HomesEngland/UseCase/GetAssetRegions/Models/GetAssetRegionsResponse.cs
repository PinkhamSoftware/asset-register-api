using System.Collections.Generic;
using HomesEngland.Domain;

namespace HomesEngland.UseCase.GetAssetRegions.Models
{
    public class GetAssetRegionsResponse
    {
        public IList<AssetRegion> Regions { get; set; }
    }
}
