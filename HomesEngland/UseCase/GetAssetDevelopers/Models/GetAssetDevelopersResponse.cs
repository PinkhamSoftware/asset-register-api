using System.Collections.Generic;
using HomesEngland.Domain;
using HomesEngland.UseCase.Models;

namespace HomesEngland.UseCase.GetAssetRegions.Models
{
    public class GetAssetDevelopersResponse : IResponse<AssetDeveloper>
    {
        public IList<AssetDeveloper> Developers { get; set; }
        public IList<AssetDeveloper> ToCsv()
        {
            return Developers;
        }
    }
}