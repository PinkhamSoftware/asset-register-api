using System.Collections.Generic;
using HomesEngland.Domain;

namespace HomesEngland.Gateway.Assets.Region
{
    public class AssetRegionsResponse
    {
        public IList<IAssetRegion> Regions { get; set; }
    }
}