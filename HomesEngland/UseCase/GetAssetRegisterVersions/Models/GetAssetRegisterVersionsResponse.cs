using System.Collections.Generic;
using HomesEngland.UseCase.Models;

namespace HomesEngland.UseCase.GetAssetRegisterVersions.Models
{
    public class GetAssetRegisterVersionsResponse : IResponse<AssetRegisterVersionOutputModel>
    {
        public IList<AssetRegisterVersionOutputModel> AssetRegisterVersions { get; set; }
        public int Pages { get; set; }
        public int TotalCount { get; set; }
        public IList<AssetRegisterVersionOutputModel> ToCsv()
        {
            return AssetRegisterVersions;
        }
    }
}
